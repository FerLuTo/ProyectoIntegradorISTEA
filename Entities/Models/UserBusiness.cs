
namespace Entities.Models
{
    public class UserBusiness
    {
        public int Id { get; set; }
        public string FantasyName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public string Slogan { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int PostalCode { get; set; }
        public string Location { get; set; } = string.Empty;
        public long Cuit { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Web { get; set; } = string.Empty;
        public bool ActiveProfile { get; set; }

        public virtual Account Account { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
