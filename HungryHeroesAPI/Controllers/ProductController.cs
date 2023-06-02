using Business.Interfaces;
using Common.Attributes;
using Entities.Enum;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace HungryHeroesAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("All")]
        public IEnumerable<ProductResponse> GetProducts()
            => _productService.GetProducts();

        [Authorize(Role.Admin)]
        //[AllowAnonymous]
        [HttpGet("AllByBusiness/{idUserBusiness}")]
        public IEnumerable<ProductResponse> GetProductsByUserBusiness(int idUserBusiness)
          => _productService.GetProductsByUserBusiness(idUserBusiness);

        
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public ProductResponse GetProduct(int id)
            => _productService.GetProduct(id);

        [Authorize(Role.Business)]
        //[AllowAnonymous]
        [HttpPost]
        public async Task<ProductResponse> Create(ProductRequest product)
          => await _productService.Create(product);

        [Authorize(Role.Business)]
        //[AllowAnonymous]
        [HttpPut("{id:int}")]
        public async Task<ProductResponse> Edit(int id, ProductRequest model)
         => await _productService.Edit(id, model);

        [Authorize(Role.Business)]
        //[AllowAnonymous]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return Ok(new { message = "Product deleted successfully" });
        }

    }
}
