using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

namespace Business.Interfaces
{
    public interface ISaleService
    {
        public IEnumerable<SaleResponse> GetAllSales();
        public SaleResponse GetSaleById(int id);
        public IEnumerable<SaleResponse> GetAllByUserClientId(int idUserClient);
        public SaleDetailResponse GetSaleDetailById(int idSaleDetail);
        public Task<SaleResponse> Create(SaleRequest sale);
        public Task<SaleResponse> Edit(int id, SaleRequest sale);
        public void Delete(int id);
    }
}
