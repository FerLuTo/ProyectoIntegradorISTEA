using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Request
{
    public class ProductRequest
    {
        [Required]
        public int UserBusinessId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public int Price { get; set; }

        public IFormFile Image { get; set; }
    }
}
