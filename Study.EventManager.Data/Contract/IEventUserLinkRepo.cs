using Study.EventManager.Data.Repositiry;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IEventUserLinkRepo : IRepository<EventUserLink>
    {
        EventUserLink GetRecordByEventAndUser(int UserId, int EventId);

        List<EventUserLink> GetAllUsers(int EventId);

        List<Event> GetEventsByUser(int userId, int page, int pageSize, string eventName, int del = 0);

        int GetEventsByUserCount(int userId, int del = 0);

        List<EventUserLink> GetCompanyUserLinkListForUser(int userId, List<int> eventIdList);

        List<User> GetListUsers(int eventId);

        int GetUserRole(int userId, int eventId);
    }
}
