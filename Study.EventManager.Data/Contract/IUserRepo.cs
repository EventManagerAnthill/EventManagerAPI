using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IUserRepo : IRepository<User>
    {
        User GetByUserEmail(string email, string password);

        bool FindEmail(string email);
    }
}

