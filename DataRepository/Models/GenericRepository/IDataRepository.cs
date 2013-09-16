using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataRepository.DataAccess.GenericRepository
{
    public interface IDataRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity FindFirstBy(Expression<Func<TEntity, bool>> predicate);
        TEntity Find(int id);
        TEntity LastRecord(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Load();
        IQueryable<TEntity> Load(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Delete(IQueryable<TEntity> entities);
        void Edit(TEntity entity);

        void Save(TEntity entity);
        int Count();
        void Refresh(TEntity entity);
    }
}
