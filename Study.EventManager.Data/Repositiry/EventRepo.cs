using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class EventRepo : AbstractRepo<Event>, IEventRepo
    {
        public EventRepo(EventManagerDbContext context)
            :base(context)
        { }
    }
}

