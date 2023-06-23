using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Response
{
    public class UserBusinessResponse
    {
        public int UserBusinessId { get; set; }
        public string FantasyName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Slogan { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int PostalCode { get; set; }
        public string Location { get; set; } = string.Empty;
        public long Cuit { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Web { get; set; } = string.Empty;
        public bool ActiveProfile { get; set; }


    }
}
