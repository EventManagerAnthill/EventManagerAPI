using Study.EventManager.Data.Repositiry;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{  
    public interface ICompanyUserLinkRepo : IRepository<CompanyUserLink>
    {
        public CompanyUserLink GetRecordByCompanyAndUser(int UserId, int CompanyId);
    }
}
