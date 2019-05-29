using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PenMgmt.Server.Persistence.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity Get (int id); 
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        Task Add (TEntity entity);
        Task AddRange (IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange (IEnumerable<TEntity> entities);
    }
}
