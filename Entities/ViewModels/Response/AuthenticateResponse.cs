using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Response
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }
        public int? UserBusinessId { get; set; }
        public int? UserClientId { get; set; }
    }
}
