using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
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
            var user = repo.GetByUserName(username, password);
            
            if (user == null)
            {
                throw new ValidationException("Incorrect Username/Password combination");                
            }

            if (user.IsVerified)
            {
                throw new ValidationException("Email incorrect");

            }

            if (user.Email == null)
            {
                throw new ValidationException("Email incorrect");
            }

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

        public UserDto CreateUser(UserCreateDto dto)
        {
            User entity = new User(dto.Username, dto.Password, dto.FirstName, dto.LastName, dto.Email);
            //var entity = MapToEntity(dto);
            var repo = _contextManager.CreateRepositiry<IUserRepo>();           
            repo.Add(entity);
            _contextManager.Save();
            return MapToDto(entity);
        }

        public UserDto UpdateUser(int id, UserDto dto)
        {
            ValidateUser(dto);

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

        //public void VerifyEmail(string email, string verificationCode)
        //{
        //    const string secretKey = "adsghfd;flasghsd;lgndfsgndfklngfsde";
        //    var email = "aaa@gmail.com";
        //    var validTo = "2021-05-20";
        //    var toVerifyCode = hash(email + "_" + validTo+ "_" + secretKey);

        //    "https://site.com/validateUser?email=aaa@gmail.com&validTo=2021-05-21&code={toVerifyCode}";
        //}

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
                Email = entity.Email,         
                Username = entity.Username
            };
        }
        private User MapToEntity(UserCreateDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                Password = dto.Password
            };
        }

        private void ValidateUser(UserDto dto)
        {
            if (dto.Email == null)
            {
                throw new ValidationException("Email incorrect");
            }
        }

    }
}