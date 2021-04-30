﻿using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface IUserService
    {
        UserDto GetUser(int id);
        UserDto CreateUser(UserDto dto);
        UserDto UpdateUser(int id, UserDto dto);
        IEnumerable<UserDto> GetAll();
        void DeleteUser(int id);
    }
}
