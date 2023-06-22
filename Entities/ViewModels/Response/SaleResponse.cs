namespace Entities.ViewModels.Response
{
    public class SaleResponse
    {
        public string Email { get; set; }
        public string BoxName { get; set; }
        public string FantasyName { get; set; }
        public string Code { get; set; }
        public DateTime DateSale { get; set; }
        public decimal Total { get; set; }
        public bool Delivered { get; set; } = false;
    }
}
