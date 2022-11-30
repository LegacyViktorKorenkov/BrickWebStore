using BrickWebStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrickWebStore.Models.ViewModels;

public class ProductViewModel
{
    public Product Product { get; set; }

    public IEnumerable<SelectListItem> CategorySelectList { get; set; }

    public IEnumerable<SelectListItem> StoreAddressList { get; set; }
}
