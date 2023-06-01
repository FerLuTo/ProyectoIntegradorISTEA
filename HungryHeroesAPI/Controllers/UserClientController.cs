using Business.Interfaces;
using Common.Attributes;
using Entities.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HungryHeroesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserClientController : ControllerBase
    {
        private readonly ISaleDetailService _saleDetailService;
        public UserClientController(ISaleDetailService saleDetailService)
        {
            saleDetailService = _saleDetailService;
        }


        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<SaleDetailResponse> GetById(int id)
         => _saleDetailService.GetById(id);

    }
}
