using Entities.Models;

namespace Entities.ViewModels.Response
{
    public class SaleResponse
    {
        public string BoxName { get; set; }
        public string FantasyName { get; set; }
        public string UserClientEmail { get; set; }
        public DateTime DateSale { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; }
        public bool Delivered { get; set; }

    }
}
