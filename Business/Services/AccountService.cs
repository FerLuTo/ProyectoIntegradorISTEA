using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common;
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
                throw new AppException("El usuario o la contraseña son incorrectos.");

            if (!account.IsVerified)
                throw new AppException("Tu cuenta no está verificada. Por favor, ingresá a tu mail para verificarla.");

            if (!account.IsActive)
                throw new KeyNotFoundException("La cuenta no está registrada.");

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

        public void Register(RegisterRequest model)
        {
            /*if (_context.Accounts.Any(x => x.Email == model.Email && x.IsActive == false))
            {
                throw new AppException("The account is already registered but inactive, do you want to reactivate it?");
            }*/
            if (_context.Accounts.Any(x => x.Email == model.Email))
            {
                throw new AppException("Este email ya está registrado.");
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
            else
            {
                CreateUserBusiness(account.Id);
            }

            SendVerificationEmail(account);

        }

        public void VerifyEmail(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token) ?? throw new AppException("Verification failed");
            account.VerifiedAt = DateTime.UtcNow;

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void ForgotPassword(ForgotPasswordRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            if (account is null || account.Email != model.Email)
                throw new AppException("El email no existe.");

            //Create a reset token that expires after 1 day
            account.ResetToken = GenerateResetToken();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _context.Accounts.Update(account);
            _context.SaveChanges();

           SendPasswordResetEmail(account);
        }

        public void ValidateResetToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                         x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow) ?? throw new AppException("Verification failed");
            
            
            account.ResetTokenExpires = null;
            account.PasswordReset = DateTime.UtcNow;
            _context.Accounts.Update(account);
            _context.SaveChanges();

        }

        public void ResetPassword(ResetPasswordRequest model)
        {

            var account = _context.Accounts.FirstOrDefault(a => a.ResetToken == model.Token);
            if (account != null)
            {
                
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);


                _context.Accounts.Update(account);
                _context.SaveChanges();
            }
        }

        public void ChangePassword(ChangePasswordRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Id == model.Id);

            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, account.PasswordHash))
                throw new AppException("La contraseña actual es incorrecta.");

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

            var userBusiness = _context.UserBusinesses.FirstOrDefault(x => x.AccountId == id);
            if (userBusiness != null)
            {
                userBusiness.IsActive = false;
                if (userBusiness.Products != null)
                    foreach (var product in userBusiness.Products.Where(x => x.UserBusinessId == userBusiness.Id))
                    {
                        product.IsActive = false;
                        _context.Products.Update(product);
                        _context.SaveChanges();

                    }
                _context.UserBusinesses.Update(userBusiness);
            }

            var userClient = _context.UserClients.FirstOrDefault(x => x.AccountId == id);
            if (userClient != null)
            {
                userClient.IsActive = false;
                _context.UserClients.Update(userClient);
            }

            _context.SaveChanges();
            SendDeletedAccountEmail(account);
        }



        #region Helper methods

        private Account GetAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account is null || account.IsActive is false)
                throw new KeyNotFoundException("Account not found");

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
                //Expires = DateTime.UtcNow.AddMinutes(15),
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

        public void SendVerificationEmail(Account account)
        {
            string confirmationUrl = $"https://hungry-heroes.vercel.app/Accounts/verify-email?token={account.VerificationToken}";
            //var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
            string message = $@"<div style=""box-sizing:border-box; width: 100vw; height: 100vh; padding: 2rem; display: flex; flex-direction: column; font-family: Roboto,Helvetica,Arial,sans-serif;"">
                             <div style=""width: 30%; align-self: center;"">
                             <img src=""https://hungryheroesstorage.blob.core.windows.net/images/logo.png"" alt="""" style=""width: 100%;height: 100%;object-fit: contain;"" />
                             </div>
                             <div style=""padding: 5rem;display: flex;flex-direction: column;align-items: center;text-align: center;color: #2C3535;"">
                             <p style=""font-size: 1.2rem;font-weight: 900;letter-spacing: -1px;"">¡Gracias por registrarte!</p>
                             <p style=""margin-top: 2rem;font-weight: 900;letter-spacing: -1px;"">
                             Por favor, <a href=""{confirmationUrl}"" style=""background-color: #B3D6F4;padding: 0.5rem;text-decoration: none;"">
                             hacé clic aquí </a>para ingresar a tu cuenta.
                             </p>
                             </div>
                             <div style=""width:100%;margin: 5rem 0 1rem 0; border-bottom: solid 1px #2C3535;""></div>
                             <div style=""font-size: 0.7rem;text-align: left;"">
                             <p>TÉRMINOS Y CONDICIONES: Lorem ipsum dolor sit, amet consectetur adipisicing elit. 
                                Facilis ea omnis nam ab voluptas sequi rem laudantium harum sunt repellat, 
                                nulla vero excepturi necessitatibus quasi sint ducimus quia commodi deleniti.
                                Lorem ipsum dolor sit, amet consectetur adipisicing elit amet consectetur. </p>
                             <p style=""font-weight: 900;letter-spacing: -0.5px;"">&copy 2023 Hungry Heroes</p>
                             </div>
                             </div>";

          

            _emailService.Send(
                 to: account.Email,
                 subject: "Hungry Heroes - Verificar Email",
                 html: message
             );


        }

        private void SendPasswordResetEmail(Account account)
        {
            string message;
          
                string confirmationUrl = $"https://hungry-heroes.vercel.app/Accounts/reset-password?token={account.ResetToken}";
                message = $@"<div style=""box-sizing: border-box;width: 100vw;height: 100vh;padding: 2rem;display: flex;flex-direction: column;font-family:Roboto,Helvetica,Arial,sans-serif;"">
                            <div styrle=""width: 30%;align-self: center;"">
                            <img src=""https://hungryheroesstorage.blob.core.windows.net/images/logo.png"" alt="""" style=""width: 100%;height: 100%;object-fit: contain;"" />
                            </div>
                            <div style=""padding: 5rem;display: flex;flex-direction: column;align-items: center;text-align: center;color: #2C3535;"">
                            <p style=""font-size: 1.2rem;font-weight: 900;letter-spacing: -1px;""> Restablecer contraseña</p>
                            <p style=""margin-top: 2rem;font-weight: 900;letter-spacing: -1px;"">Por favor, <a href=""{confirmationUrl}"" style=""background-color: #B3D6F4;padding: 0.5rem;text-decoration: none;""> hacé clic aquí </a>para restablecer tu contraseña.</p>
                            <p style=""margin-top: 2rem;font-weight: 900;letter-spacing: -1px;"">El link es válido por 24hs.</p>
                            </div>
                            <div style=""width: 100%;margin: 5rem 0 1rem 0;border-bottom: solid 1px #2C3535;""></div>
                            <div style=""font-size: 0.7rem;text-align: left;"">
                            <p>TÉRMINOS Y CONDICIONES: Lorem ipsum dolor sit, amet consectetur adipisicing elit. 
                                Facilis ea omnis nam ab voluptas sequi rem laudantium harum sunt repellat, 
                                nulla vero excepturi necessitatibus quasi sint ducimus quia commodi deleniti. 
                                Lorem ipsum dolor sit, amet consectetur adipisicing elit amet consectetur. </p>
                            <p style=""font-weight: 900;letter-spacing: -0.5px;"">&copy 2023 Hungry Heroes</p>
                            </div>
                            </div>";
            
            _emailService.Send(
                to: account.Email,
                subject: "Hungry Heroes - Reestablecer contraseña",
                html: message
            );
        }
        
       private void SendDeletedAccountEmail(Account account)
            {
                string message;

                // var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<div style=""box-sizing: border-box;width: 100vw;height: 100vh;padding: 2rem;display: flex;flex-direction: column;font-family: Roboto ,Helvetica,Arial,sans-serif;"">
                             <div style=""width: 30%;align-self: center;"">
                             <img src=""https://hungryheroesstorage.blob.core.windows.net/images/logo.png"" alt="""" style=""width: 100%;height: 100%;object-fit: contain"" ; />
                             </div>
                             <div style=""padding: 5rem;display: flex;flex-direction: column;align-items: center;text-align: center;color: #2C3535;"">
                             <p style=""margin-top: 2rem;font-weight: 900;letter-spacing: -1px;"">Tu usuario fue dado de baja.</p>
                             <p style=""margin-top: 2rem;font-weight: 900;letter-spacing: -1px;"">Lamentamos que ya no seas parte de Hungry Heroes. </p>
                             </div>
                             <div style=""width: 100%;margin: 5rem 0 1rem 0;border-bottom: solid 1px #2C3535;""></div>
                             <div style=""font-size: 0.7rem;text-align: left;"">
                             <p>TÉRMINOS Y CONDICIONES: Lorem ipsum dolor sit, amet consectetur adipisicing elit. 
                                  Facilis ea omnis nam ab voluptas sequi rem laudantium harum sunt repellat,
                                  nulla vero excepturi necessitatibus quasi sint ducimus quia commodi deleniti. 
                                  Lorem ipsum dolor sit, amet consectetur adipisicing elit amet consectetur. </p>
                            <p style=""font-weight: 900;letter-spacing: -0.5px;"">&copy 2023 Hungry Heroes</p>
                            </div>
                            </div>";

            _emailService.Send(
                    to: account.Email,
                    subject: "Hungry Heroes - Cuenta Eliminada",
                    html: message 
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
        /*
                public void ReActivateAccount(int id)
                {


                        var account = _context.Accounts.Find(id);
                        account.IsActive = true;

                        var userBusiness = _context.UserBusinesses.FirstOrDefault(x => x.AccountId == id);
                        if (userBusiness != null)
                        {
                            userBusiness.IsActive = true;
                            if (userBusiness.Products != null)
                                foreach (var product in userBusiness.Products.Where(x => x.UserBusinessId == userBusiness.Id))
                                {
                                    product.IsActive = true;
                                    _context.Products.Update(product);
                                    _context.SaveChanges();

                                }
                            _context.UserBusinesses.Update(userBusiness);
                        }

                        var userClient = _context.UserClients.FirstOrDefault(x => x.AccountId == id);
                        if (userClient != null)
                        {
                            userClient.IsActive = true;
                            _context.UserClients.Update(userClient);
                        }

                        _context.SaveChanges();




                }*/

        #endregion
    }
}
