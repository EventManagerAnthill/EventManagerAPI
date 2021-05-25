using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IUserRepo : IRepository<User>
    {
        User GetByUserName(string email, string password);
    }
}

