using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface ICompanySubRepo : IRepository<CompanySubscription>
    {
        public CompanySubscription GetCompanySubscription(int companyId);

        public bool GetStatusOfSubscription(int companyId);
    }
}
