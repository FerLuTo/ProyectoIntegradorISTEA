namespace Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int UserBusinessId { get; set; }

        public virtual UserBusiness UserBusiness { get; set; }


        //public string ImagePath { get; set; }
       
    }
}
