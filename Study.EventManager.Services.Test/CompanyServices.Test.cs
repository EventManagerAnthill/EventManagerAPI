using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;

namespace Study.EventManager.Services.Test
{
    [TestClass]
    public class CompanyServicesTest
    {
        private ICompanyService _companyService;

        public CompanyServicesTest()
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var services = new ServiceCollection();
            var settings = config.Get<Settings>();
            ContainerConfiguration.Configure(services, settings);
            var serviceProvider = services.BuildServiceProvider();
            _companyService = serviceProvider.GetService<ICompanyService>();
        }
        [TestMethod]
        public void CreateCompanyTest()
        {
            //arrange
            var company = new CompanyDto
            {
                Name = "Shyi"
            };

            //act             
            //var result = _companyService.CreateCompany();

            //assert
           // Assert.AreNotEqual(0, result.Id);
        }


        [TestMethod]
        public void DeleteCompanyTest()
        {
            _companyService.DeleteCompany(3);         
        }
    }
}
