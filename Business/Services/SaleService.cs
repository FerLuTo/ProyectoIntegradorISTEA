using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Common.Helper;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
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
        public int Create(SaleRequest model)
        {
            var product = _context.Products.Find(model.ProductId);
            var userBusiness = _context.UserBusinesses.Find(model.BusinessId);
            var userClient = _context.UserClients.Find(model.UserClientId);


                if (product.Stock <= 0)
                    throw new AppException("The product is out of stock ");

                var sale = _mapper.Map<Sale>(model);

                /*  sale.ProductId = product.Id;
                  sale.BusinessId = userBusiness.Id;
                  sale.UserClientId = userClient.Id;
                  sale.BoxName = product.Name;
                  sale.BusinessName = userBusiness.FantasyName;
                  sale.UserClientEmail = userClient.Account.Email;
                  sale.Total = model.Quantity * product.Price;
                  sale.DateSale = DateTime.UtcNow;
                  sale.Code = GenerateSaleCode();
                */

                sale.ProductId = product.Id;
                sale.BusinessId = userBusiness.Id;
                sale.UserClientId = userClient.Id;
                sale.Total = model.Quantity * product.Price;
                sale.DateSale = DateTime.Now;

                _context.Sales.Add(sale);
                _context.SaveChanges();
                
                var idSale = sale.Id;

            return idSale;
          
        }

        public SaleResponse SaleDetail(int idSale)
        {
            var sale = _context.Sales.Find(idSale);
            var userBusiness = _context.UserBusinesses.Where(x => x.Id == sale.BusinessId).Select(x => x.BusinessName);
            var product = _context.Products.Where(x => x.Id == sale.ProductId).Select(x => x.Name);
            var userClient = _context.UserClients.Where(x => x.Id == sale.UserClientId).Select(x => x.Account.Email);
            /*
            var response = _mapper.Map<SaleResponse>(sale);
            response.BusinessName = userBusiness;
            response.BoxName = product;
            response.UserClientEmail = userClient;
            response.Total = sale.Total;
            response.DateSale = sale.DateSale;
            response.Code = GenerateSaleCode();
            */

            sale.BusinessName = userBusiness.FirstOrDefault();
            sale.BoxName = product.FirstOrDefault();
            sale.UserClientEmail = userClient.FirstOrDefault();
            sale.Code = GenerateSaleCode();
            

            _context.Update(sale);
            _context.SaveChanges();

            var response = _mapper.Map<SaleResponse>(sale);
            response.Total = sale.Total;
            response.DateSale = sale.DateSale;
            
            return response;
        }
        

        public IEnumerable<SaleResponse> GetSaleByUserClientId(int idUserClient)
        {
            var sale = _context.UserClients.Find(idUserClient);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<List<SaleResponse>>(sale);
        }

        public IEnumerable<SaleResponse> GetSaleByUserBusinessId(int idUserBusiness)
        {
            var sale = _context.UserClients.Find(idUserBusiness);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<IList<SaleResponse>>(sale);
        }


        #region Helpers Methods

        /*
        private string GenerateSaleCode()
        {
            var code = Convert.ToString(RandomNumberGenerator.GetInt32(100001, 999999));

            var codeIsUnique = !_context.Sales.Any(x => x.Code == code);
            if (!codeIsUnique)
                return GenerateSaleCode();

            return code;
        }
        */

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
            var checkStock = _context.Products.Any(x => x.Stock >= quantity) ? throw new BadRequestException("Product without stock") 
                            : product.Stock -= quantity;

            product.Stock = checkStock;

            _context.Products.Update(product);
            _context.SaveChanges();
        }

        #endregion

    }
}
