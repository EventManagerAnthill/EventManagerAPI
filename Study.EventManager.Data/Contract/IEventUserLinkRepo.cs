using Study.EventManager.Data.Repositiry;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IEventUserLinkRepo : IRepository<EventUserLink>
    {
        public EventUserLink GetRecordByEventAndUser(int UserId, int EventId);

        public List<EventUserLink> GetAllUsers(int EventId);

        public List<EventUserLink> GetEventsByUser(int UserId, int del = 0);
    }
}
