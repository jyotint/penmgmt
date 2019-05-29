
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PenMgmt.Server.Persistence.Repository
{
    // FIXME: Implement Asynchronous processing
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected DbContext _dbContext { get; set; }
        
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public TEntity Get(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }  
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }
      
        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Single(predicate);
        }
        
        public async Task Add(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }


        public void Remove (TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange (IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange();
        }
    }
}
