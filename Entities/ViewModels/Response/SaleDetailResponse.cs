using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Response
{
    public class SaleDetailResponse
    {
        public string BoxName { get; set; }
        public string FantasyName { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
