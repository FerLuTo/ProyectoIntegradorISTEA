using Business.Interfaces;
using Common.Attributes;
using Common.Helper;
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

        /// <summary>
        /// Method to get all products
        /// by user business id
        /// </summary>
        /// <param name="idUserBusiness"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("AllByBusiness/{idUserBusiness}")]
        public IEnumerable<ProductResponse> GetProductsByUserBusiness(int idUserBusiness)
          => _productService.GetProductsByUserBusiness(idUserBusiness);

        /// <summary>
        /// Method to get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public ProductResponse GetProduct(int id)
            => _productService.GetProduct(id);

        /// <summary>
        /// Method to create new product
        /// used by UserBusiness only
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [Authorize(Role.Business)]
        [HttpPost]
        public async Task<ProductResponse> Create([FromForm]ProductRequest product)
        {
            if (Account.Role != Role.Business)
                throw new AppException("Unauthorized");
            return await _productService.Create(product);
        }

        /// <summary>
        /// Method to Edit a product
        /// used by UserBusiness only
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Role.Business)]
        [HttpPut("{id:int}")]
        public async Task<ProductResponse> Edit(int id, [FromForm]ProductRequest model)
        {
            if (Account.Role != Role.Business)
                throw new AppException("Unauthorized");
            return await _productService.Edit(id, model);
        }

        /// <summary>
        /// Method to Delete a product 
        /// used by UserBusiness only
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Role.Business)]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (Account.Role != Role.Business)
                return Unauthorized(new { message = "Unauthorized" });

            _productService.Delete(id);
            return Ok(new { message = "Producto eliminado." });
        }

    }
}
