using API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Study.EventManager.Data.Contract;
using Study.EventManager.Services.Contract;

namespace Study.EventManager.API.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userRepo;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _userRepo = new Mock<IUserService>();
       //     _controller = new UserController(_userRepo.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
            _controller.Users();

        }
    }
}
