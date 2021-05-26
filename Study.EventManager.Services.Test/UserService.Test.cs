using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Test
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService _userService;

        public UserServiceTest()
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var services = new ServiceCollection();
            var settings = config.Get<Settings>();
            ContainerConfiguration.Configure(services, settings);
            var serviceProvider = services.BuildServiceProvider();
            _userService = serviceProvider.GetService<IUserService>();
        }

        [TestMethod]
        public void CreateUserTest()
        {
            //arrange
            var dto = new UserCreateDto
            {
                Email = "akosotukhin@gmail.com",
                FirstName = "test",
                LastName = "test",
                Password = "123456",
            };

            //act             
            var result = _userService.CreateUser(dto);

            //assert
            Assert.AreNotEqual(0, result.Id);
        }

    }
}
