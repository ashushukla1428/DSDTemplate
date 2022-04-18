using EEONow.Context.EntityContext;
using EEONow.Interfaces;
using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Services
{
    public class Repository : IRepository
    {
        private readonly EEONowEntity context;

        public Repository()
        {
            context = new EEONowEntity();
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// generic method to insert
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public async Task<int> Insert<TEntity>(TEntity entity) where TEntity : class
        {
            context.Entry(entity).State = EntityState.Added;
            return await SaveChangesAsync();
        }

        public async Task<int> Modify<TEntity>(TEntity entity) where TEntity : class
        {
            context.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }


        public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            //return await context.Set<TEntity>().SingleOrDefaultAsync(match);
            return await context.Set<TEntity>().FirstOrDefaultAsync(match);
        }

        public async Task<List<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            return await context.Set<TEntity>().Where(match).ToListAsync();
        }

        /// <summary>
        /// generic method to delete particular record
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public async Task<int> Delete<TEntity>(TEntity entity) where TEntity : class
        {
            context.Entry(entity).State = EntityState.Deleted;
            return await SaveChangesAsync();
        }

        public IQueryable<TEntity> Get<TEntity>(params string[] entities) where TEntity : class
        {
            return context.Set<TEntity>().AsQueryable();
        }


        /// <summary>
        /// Save changes to database
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
