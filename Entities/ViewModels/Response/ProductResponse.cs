
namespace Entities.ViewModels.Response
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int Price { get; set; }

        //public string Image { get; set; }
    }
}
