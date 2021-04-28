using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.EventManager.Services.Contract;

namespace Study.EventManager.Services.Test
{
    [TestClass]
    public class CompanyServicesTest
    {
        private ICompanyService _companyService;

        public CompanyServicesTest()
        {
            var services = new ServiceCollection();
            var settings = new Settings
            {
                ConnectionString = "Server=SHYI;Database=EventManager;User Id=sa;Password=masterkey"
            };
            ContainerConfiguration.Configure(services, settings);
            var serviceProvider = services.BuildServiceProvider();
            _companyService = serviceProvider.GetService<ICompanyService>();
        }
        [TestMethod]
        public void TestMethod1()
        {
            var companyTwo = _companyService.GetCompany(3);
            var companyAll = _companyService.DeleteCompany(3);
            var company = _companyService.GetCompany(3);
            // var companyDel = _companyService.DeleteCompany(2);
            var num = 1;

        }
    }
}
