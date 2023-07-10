using Business.Interfaces;
using Common.Attributes;
using Common.Exceptions;
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
    public class SaleController : BaseController
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        /// <summary>
        /// Method to create sale 
        /// when the costumer confirms it
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [Authorize(Role.Client)]
        [HttpPost]
        public int Create(SaleRequest sale)
        {
            if (Account.Role != Role.Client)
                throw new AppException("Unauthorized");
            return _saleService.Create(sale);
        }

        /// <summary>
        /// Method to get sale by id 
        /// </summary>
        /// <param name="idSale"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetDetails/{idSale:int}")]
        public SaleResponse SaleDetail(int idSale)
        {
            return _saleService.SaleDetail(idSale);
        }

        /// <summary>
        /// Method to get all purchases made by
        /// the UserClient
        /// </summary>
        /// <param name="idUserClient"></param>
        /// <returns></returns>
        [Authorize(Role.Client)]
        [HttpGet("Buys/{idUserClient:int}")]
        public IEnumerable<SaleResponse> GetAllByUserClientId(int idUserClient)
        {
            if (Account.Role != Role.Client)
                throw new AppException("Unauthorized");
            return _saleService.GetSaleByUserClientId(idUserClient);
        }

        /// <summary>
        /// Method to get all purchases made by
        /// the UserBusiness
        /// </summary>
        /// <param name="idUserBusiness"></param>
        /// <returns></returns>
        [Authorize(Role.Business)]
        [HttpGet("GetSales/{idUserBusiness:int}")]
        public IEnumerable<SaleResponse> GetSaleByUserBusinessId(int idUserBusiness)
        {
            if (Account.Role != Role.Business)
                throw new AppException("Unauthorized");
            return _saleService.GetSaleByUserBusinessId(idUserBusiness);
        }


        /// <summary>
        /// Method to verify the sale code.
        /// When client withdraws box.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="idSale"></param>
        [Authorize(Role.Business)]
        [HttpPut("Verify-Sale")]
        public string VerifySale(string code,int idSale)
        {
            if (Account.Role != Role.Business)
                throw new AppException("Unauthorized");
           return  _saleService.VerifySale(code, idSale);
        }

        /// <summary>
        /// Method temporary to modify stock
        /// when client starts the purchase
        /// </summary>
        /// <param name="idProduct"></param>
        [Authorize(Role.Client)]
        [HttpPut("Modify-Stock")]
        public string ModifyStock(int idProduct, int quantity)
        {
            if (Account.Role != Role.Client)
                throw new AppException("Unauthorized");
            return _saleService.ModifyStock(idProduct, quantity);
        }

    }
}
