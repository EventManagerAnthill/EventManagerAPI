using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface ICompanyUserRepo : IRepository<CompanyUser>
    {
        CompanyUser GetCompanyUser(int CompanyId, int UserId);
    }
}
