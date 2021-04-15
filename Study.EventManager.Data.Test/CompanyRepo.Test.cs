using Microsoft.Extensions.DependencyInjection;
using Study.EventManager.Data.Contract;
using Study.EventManager.Data.Repositiry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Test
{
    class CompanyRepoTest
    {
        private ServiceProvider _serviceProvider;

        public CompanyRepoTest()
        {
            var service = new ServiceCollection();
            service.AddTransient<ICompanyRepo, CompanyRepo>();

            _serviceProvider = service.BuildServiceProvider();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void GetAll()
        {
            var repo = _serviceProvider.GetService<ICompanyRepo>();
            repo.GetAll();
        }

    }
}
