using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System.Security.Cryptography;


namespace Business.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public async Task<SaleResponse> Create(SaleRequest model)
        {
            var product = await _context.Products.FindAsync(model.ProductId);
            var userClient = await _context.UserClients.FindAsync(model.UserClientId);
            var userBusiness = await _context.UserBusinesses.FindAsync(model.UserBusinessId);
            if(product.Stock <= 0)
            {
                throw new BadRequestException("The product is out of stock ");
            }

            var sale = _mapper.Map<Sale>(model);
            
            sale.ProductId = product.Id;
            sale.UserClientId = userClient.Id;
            sale.UserClientEmail = userClient.Account.Email;
            sale.BusinessName = userBusiness.BusinessName;
            sale.Total = model.Quantity * product.Price;
            sale.DateSale = DateTime.UtcNow;
            sale.Code = GenerateSaleCode();
            
            _context.Sales.Add(sale);
            _context.SaveChanges();
            return _mapper.Map<SaleResponse>(model);

        }

        public IEnumerable<SaleResponse> GetSaleByUserClientId(int idUserClient)
        {
            var sale = _context.UserClients.Find(idUserClient);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<IList<SaleResponse>>(sale);
        }

        public IEnumerable<SaleResponse> GetSaleByUserBusinessId(int idUserBusiness)
        {
            var sale = _context.UserClients.Find(idUserBusiness);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<IList<SaleResponse>>(sale);
        }


        #region Helpers Methods

        private string GenerateSaleCode()
        {
            var code = Convert.ToString(RandomNumberGenerator.GetInt32(100001, 999999));

            var codeIsUnique = !_context.Sales.Any(x => x.Code == code);
            if (!codeIsUnique)
                return GenerateSaleCode();

            return code;
        }

        public void VerifySale(string code, int idSale)
        {
            var sale = _context.Sales.SingleOrDefault(x => x.Code == code && x.Id == idSale) ?? throw new BadRequestException("Verification failed");
            sale.Delivered = true;

            _context.Sales.Update(sale);
            _context.SaveChanges();
        }

        public void ModifyStock(int idProduct, int quantity)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == idProduct) ?? throw new BadRequestException("Product not found");
            var checkStock = _context.Products.Any(x => x.Stock >= quantity) ? throw new BadRequestException("Product without stock") : product.Stock -= quantity;

            product.Stock = checkStock;

            _context.Products.Update(product);
            _context.SaveChanges();
        }

        #endregion

    }
}
