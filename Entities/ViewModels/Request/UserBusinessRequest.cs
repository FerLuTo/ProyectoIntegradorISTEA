using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Request
{
    public class UserBusinessRequest
    {
        public int AccountId { get; set; }
        [Required]
        public string FantasyName { get; set; } 
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string Slogan { get; set; }
        [Required]
        public string Description { get; set; }
        public string Address { get; set; }
        [Required]
        public int PostalCode { get; set; }
        [Required]
        public string Location { get; set; } 
        public string Cuit { get; set; }
        [Required]
        public string Alias { get; set; } 
        [Required]
        public string Web { get; set; }
        public IFormFile? Image { get; set; }
    }
}
