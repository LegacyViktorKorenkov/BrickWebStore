using BrickWebStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrickWebStore.DataAccess.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
