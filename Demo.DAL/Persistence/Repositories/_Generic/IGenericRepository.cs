using Demo.DAL.Entities.Common;

namespace Demo.DAL.Persistence.Repositories._Genaric
{
    public interface IGenaricRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync(bool withAsNoTracking = true);
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllQueryable();
        //IEnumerable<T> GetAllEnumerable();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
