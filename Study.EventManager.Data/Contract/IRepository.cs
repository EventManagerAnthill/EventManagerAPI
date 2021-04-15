using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);
    }
}
