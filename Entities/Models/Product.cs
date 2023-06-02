using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public int UserBusinessId { get; set; }

        public virtual UserBusiness UserBusiness { get; set; }

        public ICollection<Sale> Sale { get; set; }

    }
}
