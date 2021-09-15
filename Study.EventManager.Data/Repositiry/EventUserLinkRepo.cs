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

        public List<Event> GetEventsByUser(int userId, int page, int pageSize, string eventName, int del = 0)
        {
            var listEvents = _eventManagerContext.EventUsers.Where(x => x.UserId == userId && x.Event.Del == del).Select(x => x.Event)
                .Where(x => x.Name.Contains(eventName) || "" == eventName)
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return listEvents;
        }

        public int GetEventsByUserCount(int userId, int del = 0)
        {
            var countEvents = _eventManagerContext.EventUsers.Where(x => x.UserId == userId && x.Event.Del == del).Select(x => x.Event).Count();
            return countEvents;
        }

        public List<EventUserLink> GetCompanyUserLinkListForUser(int userId, List<int> eventIdList)
        {
            var eventUserLinks = _eventManagerContext.EventUsers.Where(x => x.UserId == userId && eventIdList.Contains(x.EventId)).ToList();
            return eventUserLinks;
        }

        public List<User> GetListUsers(int eventId)
        {
            var eventUsers = _eventManagerContext.EventUsers.Where(x => x.EventId == eventId).Select(x => x.User).ToList();
            var eventUsesssrs = _eventManagerContext.EventUsers.Where(x => x.EventId == eventId).ToList();
            return eventUsers;
        }
    }
}
