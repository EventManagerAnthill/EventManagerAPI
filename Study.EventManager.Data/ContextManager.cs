using Study.EventManager.Data.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Study.EventManager.Data
{
    public class ContextManager : IContextManager
        
    {
        private IServiceProvider _serviceProvide;
        //private EventManagerDbContext _dbContext;
        private Dictionary<string, EventManagerDbContext> _dbContextMap;
        public ContextManager(IServiceProvider serviceProvider)
        {
            _serviceProvide = serviceProvider;
            _dbContextMap = new Dictionary<string, EventManagerDbContext>();

        }
        public T CreateRepositiry<T>(string id = "")
            where T: IRepository
        {
            var context = GetContext();
            return _serviceProvide.GetService<T>();
        }
         
        private EventManagerDbContext GetContext(string id = "")
        {
 
                if (!_dbContextMap.ContainsKey(id))
                {
                    _dbContextMap[id] = _serviceProvide.GetService<EventManagerDbContext>();
                }
                return _dbContextMap[id];
        }
        public void Save(string id = "")
        {
            GetContext(id).SaveChanges();
        }
    }


}