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

        public List<Company> GetCompaniesByUser(int UserId, int page, int pageSize, string companyName, int del = 0)
        {
            var listCompanies = _eventManagerContext.CompanyUsers.Where(x => x.UserId == UserId && x.Company.Del == del).Select(x => x.Company)
                .Where(x => x.Name.Contains(companyName) || "" == companyName)
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return listCompanies;
        }

        public int GetCompaniesByUserCount(int UserId, int del = 0)
        {
            var countCompanies = _eventManagerContext.CompanyUsers.Where(x => x.UserId == UserId && x.Company.Del == del).Select(x => x.Company).Count();
            return countCompanies;
        }

        public int GetUserRole(int userId, int companyId)
        {
            try
            {
                var userRole = _eventManagerContext.CompanyUsers.Where(x => x.UserId == userId && x.CompanyId == companyId).Select(x => x.UserCompanyRole).First();
                return userRole;
            }
            catch
            {
                return 0;
            }
        }

        public List<CompanyUserLink> GetCompanyUserLinkListForUser(int userId, List<int> companyIdList)
        {
            var companyUserLinks = _eventManagerContext.CompanyUsers.Where(x => x.UserId == userId && companyIdList.Contains(x.CompanyId)).ToList();
            return companyUserLinks;
        }

        public List<User> GetAllUsers(int CompanyId, int page, int pageSize, string firstName, string lastName)
        {
            var listUsers = _eventManagerContext.CompanyUsers.Where(x => x.CompanyId == CompanyId).Select(x => x.User)
                .Where(x => (x.FirstName.Contains(firstName) || "" == firstName) && (x.LastName.Contains(lastName) || "" == lastName))
                .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList(); 
            return listUsers;
        }

        public int GetAllUsersCount(int CompanyId)
        {
            var cntUsers = _eventManagerContext.CompanyUsers.Where(x => x.CompanyId == CompanyId).Include(x => x.User).Count();
            return cntUsers;
        }
    }
}
