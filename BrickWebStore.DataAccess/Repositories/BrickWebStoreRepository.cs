using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.Models;

namespace BrickWebStore.DataAccess.Repositories
{
    public class BrickWebStoreRepository : Repository<BrickWebStoreModel>, IBrickWebStoreRepository
    {
        private readonly AppDbContext _db;

        public BrickWebStoreRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _db = appDbContext;
        }

        public void Update(BrickWebStoreModel brickWebStoreModel)
        {
            var fromDb = base.FirstOrDefault(x => x.Id == brickWebStoreModel.Id);

            if (fromDb != null)
            {
                fromDb.ShopName = brickWebStoreModel.ShopName;
                fromDb.Address = brickWebStoreModel.Address;
            }
        }
    }
}
