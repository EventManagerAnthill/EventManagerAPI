using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class EventUserRepo : AbstractRepo<EventUser>, IEventUserRepo
    {
        public EventUser GetEventUser(int EventId, int UserId)
        {
            var EventUser = _eventManagerContext.Set<EventUser>().FirstOrDefault(x => x.EventId == EventId && x.UserId == UserId);
            return EventUser;
        }
    }
}
