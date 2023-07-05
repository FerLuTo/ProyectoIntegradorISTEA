namespace Entities.ViewModels.Request
{
    public class SaleRequest
    {
        public int UserClientId { get; set; }
        public int BusinessId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
