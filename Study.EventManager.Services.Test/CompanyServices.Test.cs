using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;

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
        public void CreateDeleteCompanyTest()
        {
            Random rng = new Random();

            //arrange
            var dto = new CompanyCreateDto
            {
                Name = "Nameasdasd",
                
                Type = 1,
                Description = "CompanyDtoDescriptionCompanyDto"

            };

            //act             
            var result = _companyService.CreateCompany(dto);
            var companyID = result.Id;
            //assert
            Assert.AreNotEqual(0, result.Id);

            _companyService.DeleteCompany(companyID);
        }
        
        [TestMethod]
        public void GetAllByCompanyUsersTest()
        {
            //act             
           // var result = _companyService.GetAllByUser("slavikyarkin@gmail.com",1,1);

            //assert
          //  Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void GetAllCompaniesByOwnerTest()
        {
            //act             
           // var result = _companyService.GetAllByOwner("slavikyarkin@gmail.com");

            //assert
            //Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void GetCompanyByIdTest()
        {
            //act             
            var result = _companyService.GetCompany(1);

            //assert
            Assert.AreNotEqual(0, result.Id);
        }

        [TestMethod]
        public void UpdateCompanyTest()
        {
            var dto = new CompanyDto
            {
                Name = "Name",       
                Type = 1,
                Description = "Description"
            };
            //act             
            var result = _companyService.UpdateCompany(4, dto);

            //assert
            Assert.AreNotEqual(0, result.Id);
        }

        [TestMethod]
        public void MakeCompanyDelTest()
        {
            var dto = new CompanyDto
            {
                Name = "Name",
                Type = 1,
                Description = "Description"
            };
            //act             
            var result = _companyService.MakeCompanyDel(1, dto);

            //assert
            Assert.AreNotEqual(0, result.Id);
        }      
    }
}
