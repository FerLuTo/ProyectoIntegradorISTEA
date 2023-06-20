using System.ComponentModel.DataAnnotations;


namespace Entities.ViewModels.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
