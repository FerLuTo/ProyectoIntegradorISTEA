using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAccountService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        void Register(RegisterRequest model);
        void VerifyEmail(string token);
        void ForgotPassword(ForgotPasswordRequest model);
        void ValidateResetToken(string token);
        void ResetPassword(ResetPasswordRequest model);
        void ChangePassword(ChangePasswordRequest model);
        AccountResponse GetById(int id);
        void Delete(int id);
        //void ReActivateAccount(int id);
    }
}
