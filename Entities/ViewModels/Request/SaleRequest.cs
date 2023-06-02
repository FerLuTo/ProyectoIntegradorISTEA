using Entities.Models;

namespace Entities.ViewModels.Request
{
    public class SaleRequest
    {
        public int ProductId { get; set; }
        public string BoxName { get; set; }
        public string FantasyName { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public Product Product { get; set; }


    }
}
