using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;

namespace Study.EventManager.Services.Test
{
    [TestClass]
    public class AuthenticateServiceTest
    {
        private IAuthenticateService _authenticateService;

        public AuthenticateServiceTest()
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var services = new ServiceCollection();
            var settings = config.Get<Settings>();
            ContainerConfiguration.Configure(services, settings);
            var serviceProvider = services.BuildServiceProvider();
            _authenticateService = serviceProvider.GetService<IAuthenticateService>();
        }

        [TestMethod]
        public void AuthenticateTest()
        {
            var result = _authenticateService.Authenticate("slavikyarkin@gmail.com", "1");
     
            Assert.AreNotEqual(0, result.Id);
        }

        [TestMethod]
        //(string email, string name, string givenName, string familyName)
        public void SocialNetworksAuthenticateTest()
        {
            var result = _authenticateService.SocialNetworksAuthenticate("slavikyarkin@gmail.com", null, "Slavik", "Yarlin");

            Assert.IsNotNull(result);            
        }      
    }
}
