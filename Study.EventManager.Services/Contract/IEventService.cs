using Study.EventManager.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface IEventService
    {
        EventDto GetEvent(int id);
        EventDto CreateEvent(EventDto dto);
        EventDto UpdateEvent(int id, EventDto dto);
        IEnumerable<EventDto> GetAll();
        EventDto GetEventsByUserId(int id);
        void DeleteEvent(int id);
        public void sendInviteEmail(int EventId, string Email);
        public string AcceptInvitation(int EventId, string Email);
        public EventDto MakeEventDel(int id, EventDto dto);
        Task UploadEventFoto(int eventId, FileDto model);
        Task DeleteEventFoto(int EventId);
    }
}
