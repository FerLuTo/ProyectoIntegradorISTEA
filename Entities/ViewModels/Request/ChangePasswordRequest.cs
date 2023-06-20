using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.Request
{
    public class ChangePasswordRequest
    { 
        public int Id { get; set; }
        //[Required]
        //public string OldPassword{ get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
