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

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _eventManagerContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
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
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                _eventManagerContext.Update(entity);


                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}
