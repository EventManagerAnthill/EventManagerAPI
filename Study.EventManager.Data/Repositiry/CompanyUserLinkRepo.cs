using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class CompanyUserLinkRepo : AbstractRepo<CompanyUserLink>, ICompanyUserLinkRepo
    {
        public CompanyUserLink GetRecordByCompanyAndUser(int UserId, int CompanyId)
        {
            var userCompanies = _eventManagerContext.Set<CompanyUserLink>().FirstOrDefault(x => x.UserId == UserId && x.CompanyId == CompanyId);
            return userCompanies;
        }
    }
}
