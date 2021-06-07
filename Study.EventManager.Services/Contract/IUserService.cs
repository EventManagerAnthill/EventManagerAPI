﻿using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface IUserService
    {
        UserDto Authenticate(string email, string password);
        UserDto GetUser(int id);
        UserDto CreateUser(UserCreateDto dto);
        UserDto UpdateUser(int id, UserDto model);
        IEnumerable<UserDto> GetAll();
        void DeleteUser(int id);
        string GetUrlToVerifyEmail(string email);
        string VerifyUrlEmail(string email, string code);
    }
}
