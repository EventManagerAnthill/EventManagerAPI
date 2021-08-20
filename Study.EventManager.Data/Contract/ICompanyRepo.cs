using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface ICompanyRepo : IRepository<Company>
    {
        Company GetAllCompaniesByUser(int UserId, int del = 0);

        Company GetCompanyByName(string Name);

        public List<User> GetCompanyUsers(int CompanyId, int del = 0);

        public int GetCompanyCountUsers(int CompanyId);
    }
}
