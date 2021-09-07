using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System.Collections.Generic;
using System.Drawing;

namespace Study.EventManager.Services.Contract
{
    public interface IEventService
    {
        EventDto GetEvent(int id);
        EventDto CreateEvent(EventCreateDto dto);
        EventDto UpdateEvent(int id, EventDto dto);
        IEnumerable<EventDto> GetAll();
        EventDto GetEventByUserId(int id);
        void DeleteEvent(int id);
        string AcceptInvitation(int EventId, string Email);
        EventDto MakeEventDel(int id, EventDto dto);
        EventDto CancelEvent(int EventId, EventDto dto);
        List<Event> GetAllByUser(string email);
        EventReviewDto EventReview(EventReviewCreateDto dto);
    }
}
