using Study.EventManager.Data.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public abstract class AbstractRepo<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly EventManagerDbContext _eventManagerContext;

        public AbstractRepo(EventManagerDbContext repositoryEventManager)
        {
            _eventManagerContext = repositoryEventManager;
        }

        public TEntity GetById(int id)
        {      
            return _eventManagerContext.Set<TEntity>().Find(id);      
        }

        public TEntity DeleteById(TEntity entity)
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

        public IQueryable<TEntity> GetAll()
        {
            return _eventManagerContext.Set<TEntity>();          
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
    }
}
