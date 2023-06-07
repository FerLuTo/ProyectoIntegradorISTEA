using Business.Interfaces;
using Common.Attributes;
using Entities.Enum;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;


namespace HungryHeroesAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _accountService.Register(model, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }

        [AllowAnonymous]
        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(VerifyEmailRequest model)
        {
            _accountService.VerifyEmail(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        /// <summary>
        /// Método para resetear la contraseña
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _accountService.ResetPassword(model);
            return Ok(new { message = "Password reset successful, you can now login" });
        }

        #region Administradores

        /// <summary>
        /// Método para ver todas las cuentas existentes por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpGet("{id:int}")]
        public ActionResult<AccountResponse> GetById(int id)
        {
            var account = _accountService.GetById(id);
            return Ok(account);
        }

        /// <summary>
        /// Método para devolver todas las cuentas que existen
        /// </summary>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpGet("All")]
        public ActionResult<IEnumerable<AccountResponse>> GetAll()
        {
            var accounts = _accountService.GetAll();
            return Ok(accounts);
        }

        /// <summary>
        /// Método para crear nuevas cuentas de administradores, sólo puede hacerlo los que tengan este rol
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpPost("Create-Admin")]
        public ActionResult<AccountResponse> Create(CreateRequest model)
        {
            var account = _accountService.Create(model);
            return Ok(account);
        }

        /// <summary>
        /// Método para eliminar cualquier cuenta.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            _accountService.Delete(id);
            return Ok(new { message = "Account deleted successfully" });
        }

        #endregion
    }

}
