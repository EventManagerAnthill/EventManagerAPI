using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IUserRepo : IRepository<User>
    {
        User GetByUserEmailPassword(string email, string password);

        User GetByUserEmail(string email);

        public List<Company> GetUserCompanies(int UserId);

        public List<Event> GetUserEvents(int UserId);
    }
}

