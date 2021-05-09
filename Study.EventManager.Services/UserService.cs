using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Study.EventManager.Services
{
    public class UserService : IUserService
    {
        private IContextManager _contextManager;

        public UserService(IContextManager contextManager)
        {
            _contextManager = contextManager;
        }

        public UserDto Authenticate(string username, string password)
        {
 
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetAll();
       
            var user = data.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null) return null;
            var result = MapToDto(user);
            return result;
        }

        public UserDto GetUser(int id)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetById(id);
            var result = MapToDto(data);
            return result;
        }

        public UserDto CreateUser(UserDto dto)
        {
            var entity = MapToEntity(dto);
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            repo.Add(entity);
            _contextManager.Save();
            return MapToDto(entity);
        }

        public UserDto UpdateUser(int id, UserDto dto)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetById(id);

            data.FirstName = dto.FirstName;
            data.LastName = dto.LastName;
            data.Middlename = dto.Middlename;
            data.BirthDate = dto.BirthDate;
            data.Email = dto.Email;
            data.Phone = dto.Phone;
            data.Sex = dto.Sex;

            _contextManager.Save();
            return MapToDto(data);
        }

        public void DeleteUser(int id)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetById(id);
            var entity = repo.Delete(data);
            _contextManager.Save();
        }

        public IEnumerable<UserDto> GetAll()
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var data = repo.GetAll();
            return data.Select(x => MapToDto(x)).ToList();
        }

        private UserDto MapToDto(User entity)
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
                Middlename = entity.Middlename,
                BirthDate = entity.BirthDate,
                Email = entity.Email,
                Phone = entity.Phone,
                Sex = entity.Sex,
                Username = entity.Username
            };
        }
        private User MapToEntity(UserDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Middlename = dto.Middlename,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                Phone = dto.Phone,
                Sex = dto.Sex,
                Username = dto.Username
            };
        }      
    }
}