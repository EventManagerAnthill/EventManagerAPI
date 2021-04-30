using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class EventService : IEventService
    {

        private IContextManager _contextManager;

        public EventService(IContextManager contextManager)
        {
            _contextManager = contextManager;
        }

        public EventDto GetEvent(int id)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);
            var result = MapToDto(data);
            return result;
        }

        public EventDto CreateEvent(EventDto dto)
        {
            var entity = MapToEntity(dto);
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            repo.Add(entity);
            _contextManager.Save();
            return MapToDto(entity);
        }

        public EventDto UpdateEvent(int id, EventDto dto)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);    
            
            data.Name = dto.Name;
            data.Type = dto.Type;
            data.CreateDate = dto.CreateDate;
            data.HoldingDate = dto.HoldingDate;
            data.User = dto.User;
            data.Description = dto.Description;

            _contextManager.Save();
            return MapToDto(data);
        }

        public void DeleteEvent(int id)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);
            var entity = repo.Delete(data);
            _contextManager.Save();
        }

        public IEnumerable<EventDto> GetAll()
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetAll();
            return data.Select(x => MapToDto(x)).ToList();
        }

        private EventDto MapToDto(Event entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new EventDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                HoldingDate = entity.HoldingDate,
                User = entity.User,
                Description = entity.Description,
                CreateDate = entity.CreateDate
            };
        }
        private Event MapToEntity(EventDto dto)
        {
            return new Event
            {
                Name = dto.Name,
                Type = dto.Type,
                HoldingDate = dto.HoldingDate,
                User = dto.User,
                Description = dto.Description,
                CreateDate = dto.CreateDate
            };
        }
    }
}

