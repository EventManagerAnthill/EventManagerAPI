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
        public User GetByUserEmailPassword(string email, string password)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }
        public User GetByUserEmail(string email)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x => x.Email == email);
            return user;
        }

        public List<Company> GetUserCompanies(int UserId)
        {
            var userCompany = _eventManagerContext.Users
                        .Include(x => x.Companies)
                        .Where(u => u.Id == UserId)
                        .FirstOrDefault();

            return userCompany.Companies.ToList();
        }

        public List<Event> GetUserEvents(int UserId)
        {
            var userEvents = _eventManagerContext.Users
                        .Include(x => x.Events)
                        .Where(u => u.Id == UserId)
                        .FirstOrDefault();

            return userEvents.Events.ToList();
        }
    }
}
