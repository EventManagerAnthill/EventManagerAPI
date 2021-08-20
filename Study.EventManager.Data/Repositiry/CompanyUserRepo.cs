using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Data.Repositiry
{
    public class CompanyUserRepo : AbstractRepo<CompanyUser>, ICompanyUserRepo
    {

        public CompanyUser GetCompanyUser(int CompanyId, int UserId)
        {
            var companyUser = _eventManagerContext.Set<CompanyUser>().FirstOrDefault(x => x.CompanyId == CompanyId && x.UserId == UserId);
            return companyUser;
        }

        public List<Company> GetListCompanies(int UserId)
        {
            var companyUser =  _eventManagerContext.Companies
                        .Include(x => x.Id)
                        .Where(x => x.User.Id == UserId)
                        .ToList();

            return companyUser;
        }

        void IRepository.SetContext(EventManagerDbContext context)
        {
            _eventManagerContext = context;
        }
    }
}
