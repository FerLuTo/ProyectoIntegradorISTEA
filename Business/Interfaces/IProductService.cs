using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductResponse> GetProducts();
        public IEnumerable<ProductResponse> GetProductsByUserBusiness(int idUserBusiness);
        public ProductResponse GetProduct(int id);
        public Task<ProductResponse> Create(ProductRequest product);
        public Task<ProductResponse> Edit(int id, ProductRequest product);
        public void Delete(int id);
        //Task<bool> IsActive(int id);
    }
}
