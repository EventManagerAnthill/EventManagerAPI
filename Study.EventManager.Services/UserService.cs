using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Study.EventManager.Services
{
    internal class UserService : IUserService
    {
        private IEmailWrapper _emailWrapper;
        private IContextManager _contextManager;
        const string secretKey = "emailVerificationHash";

        public UserService(IContextManager contextManager, IEmailWrapper emailWrapper)
        {
            _emailWrapper = emailWrapper;
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

        public string GetUrlToVerifyEmail(string email)
        {
            string hashUrl = GetHashString(secretKey + email);
            var date = DateTime.UtcNow.Date;

            date = date.AddDays(7);
            var dateStr = date.ToString("dd.MM.yyyy");
            var str = DateTime.Now.ToString("dd.MM.yyyy");           
            string url = "https://apievent.azurewebsites.net/api/user/validateUser?email=" + email + "&validTo=" + dateStr + "&code={" + hashUrl + "}";
        
            return url;
        }

        public string VerifyUrlEmail(string email, string code)
        {
            string hashUrl = "{" + GetHashString(secretKey + email) + "}";

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

        string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
 
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
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
            // string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\Study.EventManager.Services", "Resources", "WelcomeTemplate.html");
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "WelcomeTemplate.html");
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var url = GetUrlToVerifyEmail(dto.Email);
            
            var mailText = MailText.Replace("[username]", dto.FirstName + dto.LastName).Replace("[email]", dto.Email).Replace("[verifiedLink]", url);
            
            // send email 
            var emailModel = new EmailDto
            {
                Subject = $"Welcome {dto.Email}",
                Body = mailText,
                ToAddress = dto.Email,
                ToName = dto.Username
            };

            _emailWrapper.SendEmail(emailModel);
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