using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Services.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services
{
    public class ContainerConfiguration
    {
        public static void Configure(ServiceCollection serviceCollection, Settings settings)
        {
            Data.ContainerConfiguration.Configure(serviceCollection, settings.ConnectionString);
            serviceCollection.AddScoped<ICompanyService, CompanyService>();
        }
    }
}
    