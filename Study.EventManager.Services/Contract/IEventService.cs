using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface IEventService
    {
        EventDto GetEvent(int id);
        EventDto CreateEvent(EventCreateDto dto);
        EventDto UpdateEvent(int id, EventDto dto);
        IEnumerable<EventDto> GetAll();
        EventDto GetEventByUserOwnerId(int id);
        void DeleteEvent(int id);
        string AcceptInvitation(int EventId, string Email);
        EventDto MakeEventDel(int id, EventDto dto);
        EventDto CancelEvent(int EventId, EventDto dto);
        PagedEventsDto GetAllByUser(int userId, int page, int pageSize, string eventName);
        EventReviewDto EventReview(EventReviewCreateDto dto);
        Task<EventDto> UploadEventFoto(int EventId, FileDto model);
        Task<EventDto> DeleteEventFoto(int EventId);
    }
}
