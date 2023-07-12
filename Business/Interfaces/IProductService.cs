using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

namespace Business.Interfaces
{
    public interface IProductService
    {
         Task<IEnumerable<ProductResponse>> GetProductsByUserBusiness(int idUserBusiness);
        public ProductResponse GetProduct(int id);
        public Task<ProductResponse> Create(ProductRequest product);
        public Task<ProductResponse> Edit(int id, ProductRequest product);
        public void Delete(int id);
    }
}
