using Business.Interfaces;
using Business.Services;
using Common.Attributes;
using Common.Exceptions;
using Entities.Enum;
using Entities.Models;
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

        /// <summary>
        /// Method to get all business 
        /// </summary>
        /// <returns></returns>
        [Authorize(Role.Client)]
        [HttpGet("All")]
        public IEnumerable<UserBusinessResponse> GetBusiness()
        {
            if (Account.Role != Role.Client)
            {
                throw new BadRequestException("No Autorizado");
            }
              
            return _businessService.GetBusiness();
        }

        /// <summary>
        ///  Method to get business by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public UserBusinessResponse GetBusinessById(int id)
             => _businessService.GetBusinessById(id);

        /// <summary>
        /// Method to edit user profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [Authorize(Role.Business)]
        [HttpPut("{id:int}")]
        public async Task<UserBusinessResponse> Edit(int id, UserBusinessRequest model)
        {
            if (Account.Role != Role.Business)
                throw new BadRequestException("No autorizado");
            return await _businessService.Edit(id, model);
        }
    }
}
