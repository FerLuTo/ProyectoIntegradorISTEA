using Entities.Models;

namespace Entities.ViewModels.Response
{
    public class SaleResponse
    {
        //   public int ProductId { get; set; }
        //   public int BusinessId { get; set; }
        //   public int UserClientId { get; set; }
        public string BoxName { get; set; }
        public string BusinessName { get; set; }
        public string UserClientEmail { get; set; }
        public DateTime DateSale { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; }
        public bool Delivered { get; set; }

    }
}
