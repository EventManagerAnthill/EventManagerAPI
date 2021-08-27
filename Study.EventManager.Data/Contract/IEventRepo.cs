using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IEventRepo : IRepository<Event>
    {
        Event GetAllEventsByUser(int UserId);
    }
}
