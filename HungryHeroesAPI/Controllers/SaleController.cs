using Business.Interfaces;
using Common.Attributes;
using Common.Exceptions;
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
        public async Task<SaleResponse> Create(SaleRequest sale)
        {
            if (Account.Role != Role.Client)
                throw new BadRequestException("Unauthorized");
            return await _saleService.Create(sale);
        }

        /// <summary>
        /// Method to get all purchases made by
        /// the UserClient
        /// </summary>
        /// <param name="idUserClient"></param>
        /// <returns></returns>
        [Authorize(Role.Client)]
        [HttpGet("All/{idUserClient:int}")]
        public IEnumerable<SaleResponse> GetAllByUserClientId(int idUserClient)
        {
            if (Account.Role != Role.Client)
                throw new BadRequestException("Unauthorized");
            return _saleService.GetSaleByUserClientId(idUserClient);
        }

        /// <summary>
        /// Method to verify the sale code.
        /// When client withdraws box.
        /// </summary>
        /// <param name="code"></param>
        [Authorize(Role.Business)]
        [HttpPut("Verify-Sale")]
        public void VerifySale(string code,int idSale)
        {
            if (Account.Role != Role.Business)
                throw new BadRequestException("Unauthorized");
            _saleService.VerifySale(code, idSale);
        }

        /// <summary>
        /// Method temporary to modify stock
        /// when client starts the purchase
        /// </summary>
        /// <param name="idProduct"></param>
        [AllowAnonymous]
        [HttpPut("Modify-Stock")]
        public void ModifyStock(int idProduct, int quantity)
        {
            _saleService.ModifyStock(idProduct, quantity);
        }

    }
}
