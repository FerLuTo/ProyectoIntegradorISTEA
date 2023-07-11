using Entities.Models;

namespace Mailing.ViewModels
{
    public class VerificationEmail
    {
        public string VerificationToken { get; set; } = string.Empty;

        public static explicit operator VerificationEmail(Account account)
        {
            return new VerificationEmail
            {
                VerificationToken = account.VerificationToken
            };
        }
    }           
}
