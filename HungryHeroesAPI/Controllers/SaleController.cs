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
    public class SaleController : BaseController
    {
        private readonly ISaleService _saleService;
        private readonly ISaleDetailService _saleDetailService;

        public SaleController(ISaleService saleService, ISaleDetailService saleDetailService)
        {
            _saleService = saleService;
            _saleDetailService = saleDetailService;
        }

        [Authorize(Role.Client)]
        //[AllowAnonymous]
        [HttpPost]
        public async Task<SaleResponse> Create(SaleRequest sale)
            => await _saleService.Create(sale);

        [Authorize(Role.Client)]
        //[AllowAnonymous]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _saleService.Delete(id);
            return Ok(new { message = "Sale deleted successfully" });
        }

        [Authorize(Role.Client)]
        //[AllowAnonymous]
        [HttpPut("{id:int}")]
        public async Task<SaleResponse> Edit(int id, SaleRequest model)
            => await _saleService.Edit(id, model);

        [Authorize(Role.Admin)]
        //[AllowAnonymous]
        [HttpGet("All")]
        public IEnumerable<SaleResponse> GetAllSales()
            => _saleService.GetAllSales();

        [Authorize(Role.Client)]
        //[AllowAnonymous]
        [HttpGet("All/{idUserClient:int}")]
        public IEnumerable<SaleResponse> GetAllByUserClientId(int idUserClient)
            => _saleService.GetAllByUserClientId(idUserClient);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public SaleResponse GetSaleById(int id)
            => _saleService.GetSaleById(id);

        [AllowAnonymous]
        [HttpGet("{idSaleDetail:int}")]
        public IEnumerable<SaleDetailResponse> GetSaleDetailById(int idSaleDetail)
            => _saleDetailService.GetSaleDetailById(idSaleDetail);

    }
}
