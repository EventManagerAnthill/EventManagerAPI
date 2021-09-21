using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface ICompanySubRepo : IRepository<CompanySubscription>
    {
        CompanySubscription GetCompanySubscription(int companyId);

        bool GetStatusOfSubscription(int companyId);

        List<CompanySubscription> GetListOfExpiringSubs();
    }
}
