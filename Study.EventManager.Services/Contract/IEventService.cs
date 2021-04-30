using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface IEventService
    {
        EventDto GetEvent(int id);
        EventDto CreateEvent(EventDto dto);
        EventDto UpdateEvent(int id, EventDto dto);
        IEnumerable<EventDto> GetAll();
        void DeleteEvent(int id);
    }
}
