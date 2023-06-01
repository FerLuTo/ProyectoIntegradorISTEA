using Business.Interfaces;
using Common.Attributes;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HungryHeroesAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SaleController : BaseController
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<SaleResponse> Create(SaleRequest sale)
            => await _saleService.Create(sale);

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _saleService.Delete(id);
            return Ok(new { message = "Sale deleted successfully" });
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<SaleResponse> Edit(int id, SaleRequest model)
            => await _saleService.Edit(id, model);

        [AllowAnonymous]
        [HttpGet("All")]
        public IEnumerable<SaleResponse> GetAllSales()
            => _saleService.GetAllSales();

        [AllowAnonymous]
        [HttpGet("{id}")]
        public SaleResponse GetSale(int id)
            => _saleService.GetSale(id);

    }
}
