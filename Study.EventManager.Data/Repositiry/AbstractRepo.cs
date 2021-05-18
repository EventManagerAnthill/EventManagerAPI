using Study.EventManager.Data.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public abstract class AbstractRepo<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        protected EventManagerDbContext _eventManagerContext;

        public TEntity GetById(int id)
        {      
            return _eventManagerContext.Set<TEntity>().Find(id);      
        }

        public TEntity Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                _eventManagerContext.Remove(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be delete: {ex.Message}", ex);
            }            
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> criteria = null)
        {
            IQueryable<TEntity> query = _eventManagerContext.Set<TEntity>();
            if (criteria != null)
            {
                query = query.Where(criteria);
            }
            return query.ToList();          
        }

        public TEntity Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                _eventManagerContext.Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}", ex);
            }
        }

        void IRepository.SetContext(EventManagerDbContext context)
        {
            _eventManagerContext = context;
        }
    }
}
