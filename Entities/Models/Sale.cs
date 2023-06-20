namespace Entities.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BusinessName { get; set; }
        public int UserClientId { get; set; }
        public string UserClientEmail { get; set; }
        public DateTime DateSale { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; }
        public bool Delivered { get; set; }

        public UserClient UserClient { get; set; }
    }
}
