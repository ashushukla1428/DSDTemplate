using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task<int> Insert<TEntity>(TEntity entity) where TEntity : class;
        Task<int> Delete<TEntity>(TEntity entity) where TEntity : class;
        Task<int> Modify<TEntity>(TEntity entity) where TEntity : class;
        Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
        Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;
        Task<List<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>(params string[] entities) where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
