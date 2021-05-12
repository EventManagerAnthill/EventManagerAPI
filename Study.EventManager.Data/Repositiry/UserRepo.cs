using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class UserRepo : AbstractRepo<User>, IUserRepo
    {
        public User GetByUserName(string userName, string password)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x=>x.Username == userName && x.Password == password);
            return user;
        }
    }
}
