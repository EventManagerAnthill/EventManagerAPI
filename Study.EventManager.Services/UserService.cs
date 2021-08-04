using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Study.EventManager.Services
{
    internal class UserService : IUserService
    {
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;
        const string secretKey = "emailVerificationHash";

        public UserService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper)
        {         
            _generateEmailWrapper = generateEmailWrapper;            
            _contextManager = contextManager;
        }

        public UserDto Authenticate(string email, string password)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repo.GetByUserEmailPassword(email, password);

            if (user == null)
            {
                throw new ValidationException("Incorrect Username/Password combination");
            }

            if (!user.IsVerified)
            {
                throw new ValidationException("Email not verified");
            }

            ValidateUser(user.FirstName, user.LastName, user.Email);
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
           ValidateUser(dto.FirstName, dto.LastName, dto.Email);
           SendWelcomeEmail(dto);

            var repo = _contextManager.CreateRepositiry<IUserRepo>();

            var user = repo.GetByUserEmail(dto.Email);
           
            if (!(user == null))
            {
                throw new ValidationException("User with email address <" + dto.Email + "> is already exists.");
            }

            User entity = new User(dto.Username, dto.Password, dto.FirstName, dto.LastName, dto.Email);
            repo.Add(entity);
            _contextManager.Save();
           
            return MapToDto(entity);
        }

        public UserDto UpdateUser(int id, UserDto dto)
        {
            ValidateUser(dto.FirstName, dto.LastName, dto.Email);

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

        public string restorePass(string email, string password, string code)
        {
            string hashUrl = "{" + _generateEmailWrapper.GetHashString(secretKey + email) + "}";

            if (code == hashUrl)
            {
                var repo = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repo.GetByUserEmail(email);

                if (user.Password == password)
                {
                    return "The new password cannot be the same as the previous one";
                }
                user.Password = password;
                _contextManager.Save();

                return "password is restored";
            }
            return "it is not possible to restore password";
        }

        public void sendRestoreEmail(string email)
        {
            try
            {
                var repo = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repo.GetByUserEmail(email);

                if (user == null)
                {
                    throw new ValidationException("Incorrect email combination");
                }

                var generateEmail = new GenerateEmailDto
                {
                    UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/resetpassword?",
                    EmailMainText = "Password recovery",
                    ObjectId = 0
                };

                _generateEmailWrapper.GenerateEmail(generateEmail, user);
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
            }
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

        public string VerifyUrlEmail(string email, string code)
        {
            string hashUrl = "{" + _generateEmailWrapper.GetHashString(secretKey + email) + "}";

            if (code == hashUrl)
            {
                var repo = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repo.GetByUserEmail(email);
                user.IsVerified = true;
                _contextManager.Save();

                return "email is verified";
            }
            return "it is not possible to confirm email";
        }

        public void ValidateUser(string FirstName, string LastName, string Email)
        {
            if (Email == null)
            {
                throw new ValidationException("Email is incorect");
            }

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                throw new ValidationException("FirstName or LastName incorect");
            }
        }

        public void SendWelcomeEmail(UserCreateDto dto)
        {
            try
            {
                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Username = dto.Username
                };

                var generateEmail = new GenerateEmailDto
                {
                    UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/login?",
                    EmailMainText = "You are currently registered using",
                    ObjectId = 0
                };

                _generateEmailWrapper.GenerateEmail(generateEmail, user);
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
            }
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
    }
}