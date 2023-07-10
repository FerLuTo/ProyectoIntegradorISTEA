using Business.Interfaces;
using Business.Services;
using Common.Attributes;
using Common.Exceptions;
using Common.Helper;
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
                throw new AppException("Unauthorized");
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
        public async Task<UserBusinessResponse> Edit(int id, [FromForm]UserBusinessRequest model)
        {
            if (Account.Role != Role.Business)
                throw new AppException("Unauthorized");
            return await _businessService.Edit(id, model);
        }


        #region Filters
        /// <summary>
        /// Method to filter businesses by FantasyName
        /// </summary>
        /// <param name="fantasyName"></param>
        /// <returns></returns>
        [Authorize(Role.Client)]
        [HttpGet("filterByFantasyName/{fantasyName}")]
        public IEnumerable<UserBusinessResponse> FilterFantasyName(string fantasyName)
        {
            if (Account.Role != Role.Client)
                throw new AppException("Unauthorized");
            return  _businessService.FilterFantasyName(fantasyName);
        }

        /// <summary>
        /// Method to filter businesses by Location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [Authorize(Role.Client)]
        [HttpGet("filterByLocation/{location}")]
        public IEnumerable<UserBusinessResponse> FilterLocation(string location)
        {
            if (Account.Role != Role.Client)
                throw new AppException("Unauthorized");
            return  _businessService.FilterLocation(location);
        }

        #endregion
    }
}
