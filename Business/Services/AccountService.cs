using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common;
using Common.Exceptions;
using Common.Helper;
using Entities.Enum;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Business.Services
{
    public class AccountService : IAccountService
    {

        private readonly AppDBContext _context;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public AccountService(
            AppDBContext context,
            IJwtUtils jwtUtils,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            if (account == null || !BCrypt.Net.BCrypt.Verify(model.Password, account.PasswordHash))
                throw new AppException("Email or password is incorrect");

            if(!account.IsVerified)
            {
                throw new AppException("Account not verified, please check your email and follow instructions");
            }

            if(!account.IsActive)
            {
                throw new KeyNotFoundException("Account doesnt exists");
            }

            // Authentication done, generate jwt and tokens are reloaded
            var jwtToken = _jwtUtils.GenerateJwtToken(account);
            var userBusinessId = _context.UserBusinesses.FirstOrDefault(x => x.AccountId == account.Id);   
            var userClientId = _context.UserClients.FirstOrDefault(x => x.AccountId == account.Id);   

            _context.Update(account);
            _context.SaveChanges();

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.UserBusinessId = userBusinessId?.Id;
            response.UserClientId = userClientId?.Id;
            response.JwtToken = jwtToken;

            return response;
        }

        public void Register(RegisterRequest model, string origin)
        {
            if (_context.Accounts.Any(x => x.Email == model.Email))
            {/*
                if (_context.Accounts.Any(x => x.Email == model.Email && x.IsActive == false))
                {
                    var userReturn = _mapper.Map<Account>(model);
                    userReturn.IsActive = true;
                    _context.Update(userReturn);
                    _context.SaveChanges();
                }
                */
                SendAlreadyRegisteredEmail(model.Email, origin);
                return;
            }

            var account = _mapper.Map<Account>(model);

            account.Created = DateTime.UtcNow;
            account.VerificationToken = GenerateVerificationToken();
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            account.IsActive = true;


            _context.Accounts.Add(account);
            _context.SaveChanges();

            if (account.Role == Role.Client)
            {
                CreateUserClient(account.Id);
            }
            else {
                CreateUserBusiness(account.Id);
            }

            SendVerificationEmail(account, origin);
        }

        public void VerifyEmail(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token) ?? throw new BadRequestException("Verification failed");
            account.VerifiedAt = DateTime.UtcNow;

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            if (account is null) return;

            //Create a reset token that expires after 1 day
            account.ResetToken = GenerateResetToken();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _context.Accounts.Update(account);
            _context.SaveChanges();

            SendPasswordResetEmail(account, origin);
        }

        public void ValidateResetToken(ValidateResetTokenRequest model)
        {
            GetAccountByResetToken(model.Token);
        }

        public void ResetPassword(ResetPasswordRequest model)
        {
            var account = GetAccountByResetToken(model.Token);

            //Update password and remove old token
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetTokenExpires = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void ChangePassword(ChangePasswordRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Id == model.Id);

            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, account.PasswordHash))
                throw new AppException("Old password is incorrect");

            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }


        public AccountResponse GetById(int id)
        {
            var account = GetAccount(id);
            return _mapper.Map<AccountResponse>(account);
        }

        public void Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            account.IsActive = false;

            _context.Accounts.Update(account);

            var userBusiness = _context.UserBusinesses.Where(x => x.AccountId == id).ToList();
            var userClient = _context.UserClients.Where(x => x.AccountId == id).ToList();

            foreach(var entity in userBusiness)
            {
                entity.IsActive = false;
                _context.UserBusinesses.Update(entity);
            }

            foreach (var entity in userClient)
            {
                entity.IsActive = false;
                _context.UserClients.Update(entity);
            }
            _context.SaveChanges();
        }



        #region Helper methods

        private Account GetAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if(account is null ||  account.IsActive is false)
            {
                throw new KeyNotFoundException("Account not found");
            }
            return account;
        }


        private Account GetAccountByResetToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
            return account ?? throw new AppException("Invalid token");
        }

        private string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateResetToken()
        {
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            var tokenIsUnique = !_context.Accounts.Any(x => x.ResetToken == token);
            if (!tokenIsUnique)
                return GenerateResetToken();

            return token;
        }

        private string GenerateVerificationToken()
        {

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            var tokenIsUnique = !_context.Accounts.Any(x => x.VerificationToken == token);
            if (!tokenIsUnique)
                return GenerateVerificationToken();

            return token;
        }

        private void SendVerificationEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address </p>
                            <p><code>{account.VerificationToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Hungry Heroes - Verify Email",
                html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
            );
        }

        private void SendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            _emailService.Send(
                to: email,
                subject: "Hungry Heroes - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                        <p>Your email <strong>{email}</strong> is already registered.</p>
                        {message}"
            );
        }

        private void SendPasswordResetEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password </p>
                            <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Hungry Heroes - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                        {message}"
            );
        }

        private void CreateUserClient(int idAccount)
        {
            var client = new UserClient
            {
                AccountId = idAccount
            };
            client.IsActive = true;
            _context.Add(client);
            _context.SaveChanges();
        }

        private void CreateUserBusiness(int idAccount)
        {
            var business = new UserBusiness
            {
                AccountId = idAccount
            };
            business.IsActive = true;
            _context.Add(business);
            _context.SaveChanges();
        }

        #endregion
    }
}
