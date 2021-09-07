using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface IAdminService
    {
        IEnumerable<CompanyDto> GetAllCompanies();

        IEnumerable<EventDto> GetAllEvents();

        IEnumerable<UserDto> GetAllUsers();
    }
}
