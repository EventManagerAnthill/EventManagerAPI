using Microsoft.EntityFrameworkCore;
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

        public List<CompanyUserLink> GetCompaniesByUser(int UserId, int del = 0)
        {
          /*  var userCompanies = _eventManagerContext.Companies
                .Where(c => c.Users.Any(u => u.Id == UserId) && c.Del == del)
                .ToList();*/


            //var listCompanies = _eventManagerContext.CompanyUsers.Where(x => x.UserId == UserId).Include(x => x.Company.Del == del).ToList();
            var listCompanies = _eventManagerContext.CompanyUsers.Where(x => x.UserId == UserId && x.Company.Del == del).Include(x => x.Company).ToList();
            return listCompanies;
        }

        public List<CompanyUserLink> GetAllUsers(int CompanyId)
        {
            var listUsers = _eventManagerContext.CompanyUsers.Where(x => x.CompanyId == CompanyId).Include(x => x.User).ToList();
            return listUsers;
        }
    }
}
