using Study.EventManager.Data.Repositiry;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{  
    public interface ICompanyUserLinkRepo : IRepository<CompanyUserLink>
    {
        CompanyUserLink GetRecordByCompanyAndUser(int UserId, int CompanyId);

        List<Company> GetCompaniesByUser(int UserId, int page, int pageSize, int del = 0);

        int GetCompaniesByUserCount(int UserId, int del = 0);

        int GetUserRole(int userId, int companyId);
        List<CompanyUserLink> GetCompanyUserLinkListForUser(int userId, List<int> companyIdList);

        List<CompanyUserLink> GetAllUsers(int CompanyId);
    }
}
