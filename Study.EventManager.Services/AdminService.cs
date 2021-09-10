using AutoMapper;
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
        private readonly IMapper _mapper;

        public AdminService(IContextManager contextManager, IMapper mapper)
        {
            _contextManager = contextManager;
            _mapper = mapper;
        }

        public IEnumerable<EventDto> GetAllEvents()
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetAll();
            return data.Select(x => _mapper.Map<EventDto>(x)).ToList();
        }
       
        public IEnumerable<CompanyDto> GetAllCompanies()
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetAll();
            return data.Select(x => _mapper.Map<CompanyDto>(x)).ToList();
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetAll();

            return data.Select(x => _mapper.Map<UserDto>(x)).ToList(); 
        }
    }
}
