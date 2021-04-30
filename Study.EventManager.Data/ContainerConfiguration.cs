using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Data.Contract;
using Study.EventManager.Data.Repositiry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data
{
    public class ContainerConfiguration
    {
        public static void Configure(IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddScoped<ICompanyRepo, CompanyRepo>();
            serviceCollection.AddScoped<IEventRepo, EventRepo>();
            serviceCollection.AddScoped<IUserRepo, UserRepo>();
            serviceCollection.AddDbContext<EventManagerDbContext>(option => option.UseSqlServer(connectionString));
            serviceCollection.AddScoped<IContextManager, ContextManager>();
        }
    }
}
