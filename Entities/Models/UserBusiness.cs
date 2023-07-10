
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
        public string Cuit { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Web { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool ActiveProfile { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public virtual Account Account { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
