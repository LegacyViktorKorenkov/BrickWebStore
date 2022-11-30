using System.Linq.Expressions;

namespace BrickWebStore.DataAccess.Repositories.Abstractions
{
    public interface IRepository<T> where T : class
    {
        T Find(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includProperties = null, 
            bool isTracking = true);

        T FirstOrDefault(Expression<Func<T, bool>> filter = null,
            string includProperties = null,
            bool isTracking = true);

        void Add(T entity);

        void Remove(T entity);

        void Save();
    }
}
