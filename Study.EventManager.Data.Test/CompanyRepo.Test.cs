using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Data.Contract;
using Study.EventManager.Data.Repositiry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Study.EventManager.Data.Test
{
    [TestClass]
    public class CompanyRepoTest
    {
        private IContextManager _contexManager;

        public CompanyRepoTest()
        {
            var services = new ServiceCollection();            
            ContainerConfiguration.Configure(services, "Server=SHYI;Database=EventManager;User Id=sa;Password=masterkey");
            var serviceProvider = services.BuildServiceProvider();
            _contexManager = serviceProvider.GetService<IContextManager>();
        }

        [TestMethod]
        public void GetAll()
        {
             
            var repo = _contexManager.CreateRepositiry<ICompanyRepo>();            
            var c = repo.GetAll();
            c.First().Name = "aaa";

        }

        public void GetOne()
        {

            var repo = _contexManager.CreateRepositiry<ICompanyRepo>();
            repo.GetAll();
        }

    }
}
