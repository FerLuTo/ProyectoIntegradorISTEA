namespace Entities.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BusinessId { get; set; }
        public int UserClientId { get; set; }
        public string BoxName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string UserClientEmail { get; set; } = string.Empty;
        public DateTime DateSale { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool Delivered { get; set; }

        public UserClient UserClient { get; set; }
    }
}
