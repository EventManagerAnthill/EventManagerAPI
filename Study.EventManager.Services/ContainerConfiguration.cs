using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services
{
    public class ContainerConfiguration
    {
        public static void Configure(IServiceCollection serviceCollection, Settings settings)
        {
            Data.ContainerConfiguration.Configure(serviceCollection, settings.ConnectionString);
            serviceCollection.AddScoped<ICompanyService, CompanyService>();
            serviceCollection.AddScoped<IEventService, EventService>();
            serviceCollection.AddScoped<IUserService, UserService>();
        }
    } 
}
    