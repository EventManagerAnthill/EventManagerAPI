using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;

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
        string VerifyUrlEmail(string email, string code);
        public void sendRestoreEmail(string email);
        public string restorePass(string email, string password, string code);
    }
}
