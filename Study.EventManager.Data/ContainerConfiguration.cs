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
        public static void ConfigureConfiguration(ServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddScoped<ICompanyRepo, CompanyRepo>();
            serviceCollection.AddDbContext<EventManagerDbContext>(option => option.UseSqlServer(connectionString));
            serviceCollection.AddScoped<IContextManager, ContextManager>();
        }
    }
}
