using Demo.DAL.Entities.Common;
using Demo.DAL.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Persistence.Repositories._Genaric
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext;
        public GenaricRepository(ApplicationDbContext dbContext) //ask CLR for creating object from ApplicationDbContext
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync(bool withAsNoTracking = true)
        {
            //IsDeleted ==> false
            if (withAsNoTracking)
                return await _dbContext.Set<T>().Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();

            return await _dbContext.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);  //search locally, in case found => return, else => send request database  

            /// var T = _dbContext.Ts.Local.FirstOrDefault(d => d.Id == id); 
            /// if (T is null)
            ///     T = _dbContext.Ts.FirstOrDefault(d => d.Id == id);
            /// 
            /// return T;
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);  //saved locally
        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);   //change state => modified
        }
        public void Delete(T entity)
        {
            // _dbContext.Set<T>().Remove(entity);
            // return _dbContext.SaveChanges();
            // IsDeleted == true ==> deleted [Disappear from users]
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
        }
        public IQueryable<T> GetAllQueryable()
        {
            return _dbContext.Set<T>();
        }
    }
}
