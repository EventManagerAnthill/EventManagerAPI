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
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);

            if (user == null)
            {
                throw new ValidationException("Incorrect email combination");
            }

            var company = GetEvent(EventId);
            if (!(company == null))
            {
                throw new ValidationException("Company not found.");
            }

            var userEvents = repoUser.GetEventsByUser(user.Id);
            if (userEvents.Any(x => x.Id == EventId))
            {
                throw new ValidationException("User with this email is already exist in event " + company.Name);
            }

            var generateEmail = new GenerateEmailDto
            {
                UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/company/" + EventId + "?",
                EmailMainText = "Invitation to the event, for confirmation follow the link",
                ObjectId = EventId,
                Subject = "Welcome to the Event"
            };

            _generateEmailWrapper.GenerateEmail(generateEmail, user);
        }

        public string AcceptInvitation(int EventId, string Email)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);

            if (user == null)
            {
                throw new ValidationException("User not found.");
            }

            var userEvents = repoUser.GetEventsByUser(user.Id);

            var repoEvent = _contextManager.CreateRepositiry<IEventRepo>();
            var userEvent = repoEvent.GetById(EventId);

            if (userEvent == null)
            {
                throw new ValidationException("Event not found.");
            }

            if (userEvents.Any(x => x.Id == EventId))
            {
                throw new ValidationException("User is already added to the event.");
            }

            user.Events.Add(userEvent);
            _contextManager.Save();

            return "You successfully join the Event";
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
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetUserByEmail(dto.Email);

                if (user == null)
                {
                    throw new ValidationException("User not found");
                }

                var entity = MapToEntity(user.Id, dto);
                var repo = _contextManager.CreateRepositiry<IEventRepo>();
                repo.Add(entity);
                _contextManager.Save();
                return MapToDto(entity);
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
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
                Description = entity.Description,
                CreateDate = entity.CreateDate
            };
        }

        private Event MapToEntity(int id, EventDto dto)
        {
            return new Event
            {
                Name = dto.Name,
                Type = dto.Type,
                HoldingDate = dto.HoldingDate,
                UserId = id,
                Description = dto.Description,
                CreateDate = dto.CreateDate,
                CompanyId = dto.CompanyId
            };
        }
    }
}

