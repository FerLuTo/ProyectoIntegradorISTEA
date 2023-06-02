using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string BoxName { get; set; } = string.Empty;
        public string FantasyName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Total { get; set; }
        public bool Delivered { get; set; }

        public virtual Sale? Sale { get; set; }
    }
}
