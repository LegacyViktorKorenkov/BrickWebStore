using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.Models;

namespace BrickWebStore.DataAccess.Repositories
{
    public class CategotyRepository : Repository<Category>, ICategotyRepository
    {
        private readonly AppDbContext _db;

        public CategotyRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var fromDb = base.FirstOrDefault(x => x.Id == category.Id);

            if(fromDb != null)
            {
                fromDb.CategoryName = category.CategoryName;
                fromDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
