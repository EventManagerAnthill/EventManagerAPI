using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IEventRepo : IRepository<Event>
    {
        Event GetAllEventsByUser(int UserId, int del = 0);

        List<Event> GetAllEventsByCompanyId(int UserId, int page = 1, int pageSize = 20, int del = 0);

        int GetAllEventsByCompanyIdCount(int CompanyId, int del = 0);
    }
}
