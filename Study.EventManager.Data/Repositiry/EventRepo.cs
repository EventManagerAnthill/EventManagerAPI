using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class EventRepo : AbstractRepo<Event>, IEventRepo
    {
        public Event GetAllEventsByUser(int UserId)
        {
            var events = _eventManagerContext.Set<Event>().FirstOrDefault(x => x.UserId == UserId);
            return events;
        }
    }
}

