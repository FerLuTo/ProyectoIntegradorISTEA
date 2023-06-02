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
        public DateTime DateSale { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public ICollection<SaleDetail>? SaleDetails { get; set; }


    }
}
