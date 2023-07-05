﻿using Business.Interfaces;
using Common.Attributes;
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

        /// <summary>
        /// Method to login in the site
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model);

            return Ok(response);
        }

        /// <summary>
        /// Method to register new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _accountService.Register(model, Request.Headers["origin"]);
            return Ok(new { message = "Te registraste correctamente, por favor ingresá a tu mail para seguir las instrucciones" });
        }

        /// <summary>
        /// Method that sends an email with
        /// the token or link to activate an account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(VerifyEmailRequest model)
        {
            _accountService.VerifyEmail(model.Token);
            return Ok(new { message = "Bienvenido! Ahora podes loguearte" });
        }

        /// <summary>
        /// Method that when entering the email of the account 
        /// sends a token or link to restore password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new { message = "Ingresá a tu email para seguir las instrucciones." });
        }

        /// <summary>
        /// Method that expects the token sent to the mail, 
        /// new password and confirmation of the same
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _accountService.ResetPassword(model);
            return Ok(new { message = "Contraseña reestablecida, ahora podés loguearte." });
        }

        /// <summary>
        /// Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("change-password")]
        public IActionResult ChangePassword(ChangePasswordRequest model)
        {
            _accountService.ChangePassword(model);
            return Ok(new { message = "Contraseña modificada, ahora podés loguearte." });
        }

        /// <summary>
        /// Method to get accounts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public ActionResult<AccountResponse> GetById(int id)
        {
            var account = _accountService.GetById(id);
            return Ok(account);
        }

        /// <summary>
        /// Method to delete account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _accountService.Delete(id);
            return Ok(new { message = "Cuenta eliminada correctamente." });
        }
    
        /*
        /// <summary>
        /// Method to reactive account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option"></param>
        [AllowAnonymous]
        [HttpPut("ReactivateAccount/{id}")]
        public void ReActivateAccount(int id)
        {
            _accountService.ReActivateAccount(id);
        }*/
    }
}
