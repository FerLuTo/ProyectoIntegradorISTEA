using Entities.Models;

namespace Mailing.ViewModels
{
    public class ResetPasswordEmail
    {
        public string ResetToken { get; set; }

        public static explicit operator ResetPasswordEmail(Account account) 
        {
            return new ResetPasswordEmail
            {
                ResetToken = account.ResetToken
            };
        }
    }
}
