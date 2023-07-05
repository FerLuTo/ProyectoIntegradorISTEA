
namespace Entities.ViewModels.Response
{
    public class ProductResponse
    {
        public int UserBusinessId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }

        //public string Image { get; set; }
    }
}
