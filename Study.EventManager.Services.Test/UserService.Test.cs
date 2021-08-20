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
        public void CreateDeleteUserTest()
        {
            Random rng = new Random();

            //arrange
            var dto = new UserCreateDto
            {
                Email = rng.Next(10) + "shyi2517" + rng.Next(10) + rng.Next(10) + rng.Next(10) + "@gmail.com",
                FirstName = "test",
                LastName = "test",
                Password = "testR125454",
            };

            //act             
            var result = _userService.CreateUser(dto);
            var userID = result.Id;
            //assert
            Assert.AreNotEqual(0, result.Id);

            _userService.DeleteUser(userID);
        }

        [TestMethod]
        public void GetAllUsersTest()
        {
            //act             
            var result = _userService.GetAll();

            //assert
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            //act             
            var result = _userService.GetUser(1);

            //assert
            Assert.AreNotEqual(0, result.Id);
        }

        [TestMethod]
        public void UpdateUserTest()
        {

            var dto = new UserDto
            {
                Email = "sadasd@gmail.com",
                FirstName = "test",
                LastName = "test",
                Middlename = "Middlename",                         
                Phone = "8989632332"              

            };
            //act             
            var result = _userService.UpdateUser(1, dto);

            //assert
            Assert.AreNotEqual(0, result.Id);


        }
    }
}
