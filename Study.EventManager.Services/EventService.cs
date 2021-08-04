using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Study.EventManager.Services
{
    internal class EventService : IEventService
    {
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;

        public EventService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
        }

        public void sendInviteEmail(int EventId, string Email)
        {
            try
            {
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetByUserEmail(Email);

                if (user == null)
                {
                    throw new ValidationException("Incorrect email combination");
                }

                var repEventUser = _contextManager.CreateRepositiry<IEventUserRepo>();
                var eventUser = repEventUser.GetEventUser(EventId, user.Id);

                if (!(eventUser == null))
                {
                    var eventUserModel = GetEvent(EventId);
                    throw new ValidationException("User with this email is already exist in company " + eventUserModel.Name);
                }

                var generateEmail = new GenerateEmailDto
                {
                    UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/company/" + EventId + "?",
                    EmailMainText = "Invitation to the company, for confirmation follow the link",
                    ObjectId = EventId
                };

                _generateEmailWrapper.GenerateEmail(generateEmail, user);

            }
            catch
            {
                throw new CompanyExceptions("Sorry, unexpected error.");
            }
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
            try
            {
                var entity = MapToEntity(dto);
                var repo = _contextManager.CreateRepositiry<IEventRepo>();
                repo.Add(entity);
                _contextManager.Save();
                return MapToDto(entity);
            }
            catch
            {
                throw new EventExceptions("Sorry, unexpected error.");
            }
        }

        public EventDto UpdateEvent(int id, EventDto dto)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);    
            
            data.Name = dto.Name;
            data.Type = dto.Type;
            data.CreateDate = dto.CreateDate;
            data.HoldingDate = dto.HoldingDate;
            data.UserId = dto.User;
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

        public EventDto GetEventsByUserId(int UserId)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetAllEventsByUser(UserId);
            var result = MapToDto(data);
            return result;
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
                User = entity.UserId,
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
                UserId = dto.User,
                Description = dto.Description,
                CreateDate = dto.CreateDate
            };
        }
    }
}

