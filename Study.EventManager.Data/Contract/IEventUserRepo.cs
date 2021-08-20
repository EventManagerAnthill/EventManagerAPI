using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IEventUserRepo : IRepository<EventUser>
    {
        EventUser GetEventUser(int EventId, int UserId);
    }
}
