using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISaleService
    {
        public IEnumerable<SaleResponse> GetAllSales();
        public SaleResponse GetSale(int id);
        public Task<SaleResponse> Create(SaleRequest sale);
        public Task<SaleResponse> Edit(int id, SaleRequest sale);
        public void Delete(int id);
    }
}
