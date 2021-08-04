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
        public Company GetAllCompaniesByUser(int UserId, int del = 0)
        {
            var companies = _eventManagerContext.Set<Company>().FirstOrDefault(x => x.UserId == UserId && x.Del == del);
            return companies;
        }

        public Company GetCompanyByName(string Name)
        {
            var company = _eventManagerContext.Set<Company>().FirstOrDefault(x => x.Name == Name);
            return company;
        }        
    }
}
