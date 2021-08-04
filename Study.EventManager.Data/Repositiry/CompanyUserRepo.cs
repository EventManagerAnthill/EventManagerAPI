using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class CompanyUserRepo : AbstractRepo<CompanyUser>, ICompanyUserRepo
    {
        public CompanyUser GetCompanyUser(int CompanyId, int UserId)
        {
            var companyUser = _eventManagerContext.Set<CompanyUser>().FirstOrDefault(x => x.CompanyId == CompanyId && x.UserId == UserId);
            return companyUser;
        }
    }
}
