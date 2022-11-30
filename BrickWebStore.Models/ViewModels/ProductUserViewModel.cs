using BrickWebStore.Models;

namespace BrickWebStore.Models.ViewModels;

public class ProductUserViewModel
{
    public ProductUserViewModel()
    {
        Products = new List<Product>();
    }

    public ShopUser User { get; set; }
    public IList<Product> Products { get; set;}
}
