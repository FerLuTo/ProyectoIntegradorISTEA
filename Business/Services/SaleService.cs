using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public async Task<SaleResponse> Create(SaleRequest model)
        {
            _ = await _context.UserBusinesses.FindAsync(model.UserClientId) ?? throw new NotFoundException("User Client not found");

            var sale = _mapper.Map<Sale>(model);

            await _context.SaveChangesAsync();

            return _mapper.Map<SaleResponse>(sale);
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

        public SaleResponse GetSale(int id)
        {
            var sale = _context.Sales.Find(id);
            return sale is null ? throw new NotFoundException("Sale doesn't exists") : _mapper.Map<SaleResponse>(sale);
        }
    }
}
