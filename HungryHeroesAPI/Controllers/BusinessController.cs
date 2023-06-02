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
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;
        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [Authorize(Role.Business)]
       // [AllowAnonymous]
        [HttpPost]
        public async Task<UserBusinessResponse> Create(UserBusinessRequest model)
            => await _businessService.Create(model);

        [Authorize(Role.Business)]
       // [AllowAnonymous]
        [HttpPut("{id:int}")]
        public async Task<UserBusinessResponse> Edit(int id, UserBusinessRequest model)
            => await _businessService.Edit(id, model);


    }
}
