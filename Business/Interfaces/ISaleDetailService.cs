using Entities.ViewModels.Response;

namespace Business.Interfaces
{
    public interface ISaleDetailService
    {
        IEnumerable<SaleDetailResponse> GetSaleDetailById(int id);
    }
}
