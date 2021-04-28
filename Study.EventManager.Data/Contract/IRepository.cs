using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IRepository
    {

    }
    public interface IRepository<TEntity> : IRepository
        where TEntity : class, new()
    {
        TEntity GetById(int id);

        TEntity DeleteById(TEntity entity);

        IQueryable<TEntity> GetAll();

        TEntity Add(TEntity entity);
    }
}
