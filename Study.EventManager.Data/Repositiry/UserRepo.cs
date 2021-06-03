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
        public User GetByUserEmail(string email, string password)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public bool FindEmail(string email)
        {
            var user = _eventManagerContext.Set<User>().FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

    }
}
