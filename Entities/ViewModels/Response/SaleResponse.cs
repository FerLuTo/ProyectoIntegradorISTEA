using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Response
{
    public class SaleResponse
    {
        public string BoxName { get; set; }
        public string FantasyName { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
