using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
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

        public async Task<SaleResponse> Create(SaleRequest model)
        {/*
            _ = await _context.UserBusinesses.FindAsync(model.UserClientId) ?? throw new NotFoundException("User Client not found");

            var sale = _mapper.Map<Sale>(model);

            await _context.SaveChangesAsync();

            return _mapper.Map<SaleResponse>(sale);
            */

            if (_context.Sales is null)
            {
                throw new BadRequestException("Cannot enter new sale");
            }

            Product newProd = _context.Products.Find(model.ProductId);
            Sale newSale = new Sale();
            newSale.DateSale = DateTime.Now;
            newSale.Product = newProd;
            newSale.Quantity = model.Quantity;
            newSale.Total = model.Product.Price * model.Quantity;
            newSale.Code = generateSaleCode();
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync();

            var product = await _context.Products.FindAsync(model.ProductId);
            if (product != null)
            {
                product.Stock -= model.Quantity;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<SaleResponse>(model);
        }

        public void Delete(int id)
        {
            var sale = _context.Sales.Find(id) ?? throw new NotFoundException("Sale doesn't exist");
            _context.Sales.Remove(sale);
            _context.SaveChanges();

        }

        public async Task<SaleResponse> Edit(int id, SaleRequest model)
        {
            var sale = await _context.Sales.FindAsync(id) ?? throw new NotFoundException("Sale doesn't exist");
            _mapper.Map(model, sale);
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();

            return _mapper.Map<SaleResponse>(sale);
        }

        public IEnumerable<SaleResponse> GetAllSales()
        {
            var sales = _context.Sales;

            return sales is null ? throw new NotFoundException("There's no sales yet") : _mapper.Map<IList<SaleResponse>>(sales);
        }

        public SaleResponse GetSaleById(int id)
        {
            var sale = _context.Sales.Find(id);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<SaleResponse>(sale);
        }

        public IEnumerable<SaleResponse> GetAllByUserClientId(int idUserClient)
        {
            var sale = _context.UserClients.Find(idUserClient);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<IList<SaleResponse>>(sale);
        }

        public SaleDetailResponse GetSaleDetailById(int idSaleDetail)
        {
            var saleDetail = _context.SaleDetails.Find(idSaleDetail);
            return saleDetail is null ? throw new NotFoundException("Sale detail doesn't exists") : _mapper.Map<SaleDetailResponse>(saleDetail);
        }
        private string generateSaleCode()
        {
            // Code aleatorio criptográficamente fuerte
            var code = Convert.ToString(RandomNumberGenerator.GetBytes(64));

            // Se asegura que el code es unico chequeando en la DB
            var codeIsUnique = !_context.Sales.Any(x => x.Code == code);
            if (!codeIsUnique)
                return generateSaleCode();

            return code;
        }


    }
}
