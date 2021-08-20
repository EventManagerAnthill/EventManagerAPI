using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Study.EventManager.Data.Repositiry
{

    public class UserRepo : AbstractRepo<User>, IUserRepo
    {
        public User GetUserByEmailPassword(string email, string password)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x => x.Email == email);
            return user;
        }

        public List<Company> GetCompaniesByUser(int UserId, int del = 0)
        {
            var userCompanies = _eventManagerContext.Companies
                .Where(c => c.Users.Any(u => u.Id == UserId) && c.Del == del)
                .ToList();           

            return userCompanies;
        }

        public List<Event> GetEventsByUser(int UserId)
        {
            var userEvents = _eventManagerContext.Events
              .Where(c => c.Users.Any(u => u.Id == UserId))
              .ToList();

            return userEvents;
        }
    }
}
