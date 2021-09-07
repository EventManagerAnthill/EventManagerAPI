using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface ICompanyRepo : IRepository<Company>
    {
        List<Company> GetAllCompaniesByOwner(int UserId, int page = 1, int pageSize = 20, int del = 0);

        public int GetAllCompaniesByOwnerCount(int UserId, int del = 0);

        Company GetCompanyByName(string Name);
    }
}
