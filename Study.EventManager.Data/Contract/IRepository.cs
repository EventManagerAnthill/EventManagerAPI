using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IRepository
    {
        internal void SetContext(EventManagerDbContext context);
    }
    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        TEntity GetById(int id);

        TEntity Delete(TEntity entity);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> criteria = null);

        TEntity Add(TEntity entity);
    }
}
