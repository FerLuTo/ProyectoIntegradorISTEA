using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Common.Helper;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


namespace Business.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public SaleService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(SaleRequest model)
        {
            var product = await _context.Products.FindAsync(model.ProductId);
            var userBusiness = await _context.UserBusinesses.FindAsync(model.BusinessId);
            var userClient = await _context.UserClients.FindAsync(model.UserClientId);

            var sale = _mapper.Map<Sale>(model);
            sale.ProductId = product.Id;
            sale.BusinessId = userBusiness.Id;
            sale.UserClientId = userClient.Id;
            sale.Total = model.Quantity * product.Price;
            sale.DateSale = DateTime.Now;

            // TODO: PROBAR
            await ModifyStock(sale.ProductId, sale.Quantity);

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var idSale = sale.Id;

            return idSale;
        }

        public SaleResponse SaleDetail(int idSale)
        {
            var sale = _context.Sales.Find(idSale);
            var userBusiness = _context.UserBusinesses.Where(x => x.Id == sale.BusinessId).Select(x => x.FantasyName);
            var product = _context.Products.Where(x => x.Id == sale.ProductId).Select(x => x.Name);
            var userClient = _context.UserClients.Where(x => x.Id == sale.UserClientId).Select(x => x.Account.Email);

            sale.FantasyName = userBusiness.First();
            sale.BoxName = product.First();
            sale.UserClientEmail = userClient.First();
            sale.Code = GenerateSaleCode();
            
            _context.Update(sale);
            _context.SaveChanges();

            var response = _mapper.Map<SaleResponse>(sale);
            
            return response;
        }


        public IEnumerable<SaleResponse> GetSaleByUserClientId(int idUserClient)
        {
            
            var sale = _context.Sales.Where(x => x.UserClientId == idUserClient).OrderBy(x => x.DateSale).ToList();

            return _mapper.Map<IList<SaleResponse>>(sale);
           
        }

        public IEnumerable<SaleResponse> GetSaleByUserBusinessId(int idUserBusiness)
        {
            var sale = _context.Sales.Where(x => x.BusinessId == idUserBusiness).OrderBy(x => x.DateSale).ToList();
            return _mapper.Map<IList<SaleResponse>>(sale);
        }


        #region Helpers Methods

        private string GenerateSaleCode()
        {
            const int minCode = 100001;
            const int maxCode = 999999;

            string code;
            bool codeIsUnique;

            do
            {
                code = GenerateRandomCode(minCode, maxCode);
                codeIsUnique = IsCodeUnique(code);
            } while (!codeIsUnique);

            return code;
        }

        private string GenerateRandomCode(int minValue, int maxValue)
        {
            Random random = new Random();
            int code = random.Next(minValue, maxValue + 1);
            return code.ToString();
        }

        private bool IsCodeUnique(string code)
        {
            return !_context.Sales.Any(x => x.Code == code);
        }



        public string VerifySale(string code, int idSale)
        {
            var message = "Ok";
            var sale = _context.Sales.First(x => x.Code == code && x.Id == idSale) ?? throw new AppException("El código ingresado es incorrecto");
            sale.Delivered = true;

            _context.Sales.Update(sale);
            _context.SaveChanges();

            return message;
        }

        public async Task ModifyStock(int idProduct, int quantity)
        {
            var product = await _context.Products.FindAsync(idProduct);

            product.Stock -= quantity;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        #endregion

    }
}
