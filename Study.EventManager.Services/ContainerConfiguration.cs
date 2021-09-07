using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Wrappers;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services
{
    public class ContainerConfiguration
    {
        public static void Configure(IServiceCollection serviceCollection, Settings settings)
        {
            serviceCollection.AddAutoMapper(typeof(ContainerConfiguration));

            Data.ContainerConfiguration.Configure(serviceCollection, settings.ConnectionString);
            serviceCollection.AddSingleton(settings);
            serviceCollection.AddScoped<ICompanyService, CompanyService>();
            serviceCollection.AddScoped<IEventService, EventService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IAuthenticateService, AuthenticateService>();
            serviceCollection.AddTransient<IEmailWrapper, SendBoxWrapper>();
            serviceCollection.AddScoped<IUploadService , UploadService >();
            serviceCollection.AddScoped<IAdminService, AdminService>();       
            serviceCollection.AddTransient<IGenerateEmailWrapper, GenerateEmailWrapper>();
            serviceCollection.AddTransient<IAuthenticateWrapper, AuthenticateWrapper>();          
            serviceCollection.AddTransient<IGenerateQRCode, GenerateQRCodeWrapper>();
        }
    } 
}
    