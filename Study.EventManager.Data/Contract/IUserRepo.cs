using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IUserRepo : IRepository<User>
    {
        User GetUserByEmailPassword(string email, string password);

        User GetUserByEmail(string email);

        public List<Company> GetCompaniesByUser(int UserId, int del = 0);

        public List<Event> GetEventsByUser(int UserId);
    }
}

