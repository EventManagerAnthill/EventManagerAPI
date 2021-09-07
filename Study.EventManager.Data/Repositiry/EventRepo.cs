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
        public Event GetAllEventsByUser(int UserId, int del = 0)
        {
            var events = _eventManagerContext.Set<Event>().FirstOrDefault(x => x.UserId == UserId && x.Del == del);
            return events;
        }

        public List<Event> GetAllEventsByCompanyId(int CompanyId, int page, int pageSize, int del = 0)
        {
            var events = _eventManagerContext.Set<Event>().Where(x => x.CompanyId == CompanyId && x.Del == del)
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return events;
        }

        public int GetAllEventsByCompanyIdCount(int CompanyId, int del = 0)
        {
            var events = _eventManagerContext.Set<Event>().Where(x => x.CompanyId == CompanyId && x.Del == del).Count();
            return events;
        }
    }
}

