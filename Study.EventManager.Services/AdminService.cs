using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Services
{
    internal class AdminService : IAdminService
    {
        private IContextManager _contextManager;

        public AdminService(IContextManager contextManager)
        {
            _contextManager = contextManager;
        }

        public IEnumerable<EventDto> GetAllEvents()
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetAll();
            return data.Select(x => EventMapToDto(x)).ToList();
        }
       
        public IEnumerable<CompanyDto> GetAllCompanies()
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetAll();
            return data.Select(x => CompanyMapToDto(x)).ToList();
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetAll();
            return data.Select(x => UserMapToDto(x)).ToList();
        }

        private EventDto EventMapToDto(Event entity)
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
                CreateDate = entity.CreateDate,
                Del = entity.Del
            };
        }

        private CompanyDto CompanyMapToDto(Company entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new CompanyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UserId = entity.UserId,
                Type = entity.Type,
                Description = entity.Description,
                Del = entity.Del
            };
        }

        private UserDto UserMapToDto(User entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new UserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Username = entity.Username,
                BirthDate = entity.BirthDate,
                FotoUrl = entity.FotoUrl,
                MiddleName = entity.Middlename,
                Phone = entity.Phone,
                Sex = entity.Sex
            };
        }
    }
}
