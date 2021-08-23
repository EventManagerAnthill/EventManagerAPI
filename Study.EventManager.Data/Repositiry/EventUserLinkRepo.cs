using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class EventUserLinkRepo : AbstractRepo<EventUserLink>, IEventUserLinkRepo
    {
        public EventUserLink GetRecordByEventAndUser(int UserId, int EventId)
        {
            var userEvents = _eventManagerContext.Set<EventUserLink>().FirstOrDefault(x => x.UserId == UserId && x.EventId == EventId);
            return userEvents;
        }

        public List<EventUserLink> GetAllUsers(int EventId)
        {
            var listUsers = _eventManagerContext.EventUsers.Where(x => x.EventId == EventId).Include(x => x.User).ToList();
            return listUsers;
        }
    }
}
