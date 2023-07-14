using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

namespace Business.Interfaces
{
    public interface ISaleService
    {
        IEnumerable<SaleResponse> GetSaleByUserClientId(int idUserClient);
        IEnumerable<SaleResponse> GetSaleByUserBusinessId(int idUserBusiness);
        Task<int> Create(SaleRequest model);
        SaleResponse SaleDetail(int idSale);
        string VerifySale(string code);
        Task ModifyStock(int idProduct, int quantity);

    }
}
