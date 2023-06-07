using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int UserClientId { get; set; }
        public DateTime DateSale { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public UserClient UserClient { get; set; }

        public ICollection<SaleDetail>? SaleDetails { get; set; }


    }
}
