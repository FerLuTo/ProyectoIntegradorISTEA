using Entities.Enum;

namespace Entities.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public string? VerificationToken { get; set; } 
        public DateTime? VerifiedAt { get; set; }
        public bool IsVerified => VerifiedAt.HasValue || PasswordReset.HasValue;
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }

        
        public virtual ICollection<UserBusiness>? UsersBusiness { get; set; }
        public virtual ICollection<UserClient>? UsersClients { get; set; }
        
    }
}
