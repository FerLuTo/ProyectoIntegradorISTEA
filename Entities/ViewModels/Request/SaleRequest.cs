using Entities.Models;

namespace Entities.ViewModels.Request
{
    public class SaleRequest
    {
        public int UserClientId { get; set; }
        public string BoxName { get; set; }
        public string FantasyName { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public ICollection<SaleDetail>? SaleDetails { get; set; }

    }
}
