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
        public string FantasyName { get; set; }
        public string BusinessName { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; } 
        public string Address { get; set; } 
        public int PostalCode { get; set; }
        public string Location { get; set; } 
        public string Cuit { get; set; } 
        public string Alias { get; set; }
        public string Web { get; set; }
        public bool ActiveProfile { get; set; }

        public string ImageUrl { get; set; }
    }
}
