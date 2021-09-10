using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class CompanyRepo : AbstractRepo<Company>, ICompanyRepo
    {
        public List<Company> GetAllCompaniesByOwner(int UserId, int page, int pageSize, string companyName, int del = 0)
        {
            var companies = _eventManagerContext.Set<Company>()
                .Where(x => (x.UserId == UserId && x.Del == del) && (x.Name.Contains(companyName) || "" == companyName))
                .OrderBy(x => x.Name)
                .Skip(0 * pageSize)
                .Take(pageSize)
                .ToList();
            return companies;
        }

        public int GetAllCompaniesByOwnerCount(int UserId, int del = 0)
        {
            var companiesCount = _eventManagerContext.Set<Company>().Where(x => x.UserId == UserId && x.Del == del).Count();
            return companiesCount;
        }

        public Company GetCompanyByName(string Name)
        {
            var company = _eventManagerContext.Set<Company>().FirstOrDefault(x => x.Name == Name);
            return company;
        }
    }
}
