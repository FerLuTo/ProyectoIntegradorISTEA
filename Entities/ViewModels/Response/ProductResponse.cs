using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Response
{
    public class ProductResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int Price { get; set; }

        public string Image { get; set; }
        public string TechnicalDataSheet { get; set; }
    }
}
