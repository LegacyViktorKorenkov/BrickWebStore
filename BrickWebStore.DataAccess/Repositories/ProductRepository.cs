using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.Models;
using BrickWebStore.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BrickWebStore.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _db = dbContext;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            IEnumerable<SelectListItem> result = null;

            switch (obj)
            {
                case WC.CategoryName:
                    result = _db.Category.Select(x => new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.Id.ToString(),
                    });
                    break;
                case WC.BrickStoreName:
                    result = _db.BrickWebStoreModel.Select(x => new SelectListItem
                    {
                        Text = $"Store name: {x.ShopName}, address: {x.Address}",
                        Value = x.Id.ToString(),
                    });
                    break;
            }

            return result;
        }

        public void Update(Product product)
        {
            _db.Product.Update(product);
        }
    }
}
