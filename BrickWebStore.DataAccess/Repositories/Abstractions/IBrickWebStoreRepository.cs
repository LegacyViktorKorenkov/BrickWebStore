using BrickWebStore.Models;

namespace BrickWebStore.DataAccess.Repositories.Abstractions
{
    public interface IBrickWebStoreRepository : IRepository<BrickWebStoreModel>
    {
        void Update(BrickWebStoreModel brickWebStoreModel);
    }
}
