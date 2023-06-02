using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Entities.ViewModels.Response;

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

        public IEnumerable<SaleDetailResponse> GetSaleDetailById(int id)
        {
            var sale = _context.Sales.Find(id);
            var saleDetail = _context.SaleDetails;
            return sale is null ? throw new NotFoundException("Sale doesnt exists") : _mapper.Map<IList<SaleDetailResponse>>(saleDetail);
        }
    }
}
