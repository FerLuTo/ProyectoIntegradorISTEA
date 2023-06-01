using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class SaleDetailService : ISaleDetailService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public SaleDetailService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<SaleDetailResponse> GetById(int id)
        {
            var sale = _context.Sales.Find(id);
            var saleDetail = _context.SaleDetails;
            return sale is null ? throw new NotFoundException("Sale doesnt exists") : _mapper.Map<IList<SaleDetailResponse>>(saleDetail);
        }
    }
}
