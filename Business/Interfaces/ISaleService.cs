using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

namespace Business.Interfaces
{
    public interface ISaleService
    {
        IEnumerable<SaleResponse> GetSaleByUserClientId(int idUserClient);
        IEnumerable<SaleResponse> GetSaleByUserBusinessId(int idUserBusiness);
        int Create(SaleRequest model);
        SaleResponse SaleDetail(int idSale);
        void VerifySale(string code, int idSale);
        void ModifyStock(int idProduct, int quantity);

    }
}
