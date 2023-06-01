using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserClient
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public bool  IsActive { get; set; }
        public virtual Account? Account { get; set; }
        public ICollection<Sale>? Sales { get; set; }

    }
}
