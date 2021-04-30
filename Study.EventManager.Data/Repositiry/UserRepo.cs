using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class UserRepo : AbstractRepo<User>, IUserRepo
    {
        public UserRepo(EventManagerDbContext context)
            : base(context)
        { }
    }
}
