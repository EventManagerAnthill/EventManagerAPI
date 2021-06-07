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

    }
}
