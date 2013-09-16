using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace DataRepository.DataAccess.GenericRepository
{

    public class DataRepository<TEntity> : IDataRepository<TEntity> 
        where TEntity : class, IEntityId
    {
        private readonly DbContext _context;

        public DataRepository(DbContext context) {
            _context = context;
        }

        public virtual TEntity FindFirstBy(Expression<Func<TEntity, bool>> predicate) {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> Load(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public virtual void Add(TEntity entity) {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(TEntity entity) {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Delete(IQueryable<TEntity> entities) {
            foreach (TEntity entity in entities.ToList())
                _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Edit(TEntity entity) {
            _context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public void Save(TEntity entity) {
            if (entity.Id == 0) {
                _context.Set<TEntity>().Add(entity);
            } else {
                _context.Entry(entity).State = System.Data.EntityState.Modified;
            }

            _context.SaveChanges();
        }

        public virtual TEntity Find(int id) {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity LastRecord(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).OrderByDescending(t => t.Id).First();
        }

        public IQueryable<TEntity> Load()
        {
            return _context.Set<TEntity>();
        }

        public void Refresh(TEntity entity) {
            _context.Entry(entity).Reload();
        }

        public virtual int Count() {
            return _context.Set<TEntity>().Count();
        }

        public void Dispose() {
            if (_context != null)
                _context.Dispose();
        }
    }
}
