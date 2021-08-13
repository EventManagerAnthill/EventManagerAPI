using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;

namespace Study.EventManager.Services.Contract
{
    public interface IUserService
    {        
        UserDto GetUser(int id);
        UserDto CreateUser(UserCreateDto dto);
        UserDto UpdateUser(int id, UserDto model);
        IEnumerable<UserDto> GetAll();
        void DeleteUser(int id);              
    }
}
