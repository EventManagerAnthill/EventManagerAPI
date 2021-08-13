using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Study.EventManager.Services
{
    internal class AuthenticateService : IAuthenticateService
    {
        const string secretKey = "emailVerificationHash";
        private IGenerateEmailWrapper _generateEmailWrapper;        
        private IContextManager _contextManager;

        public AuthenticateService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper)
        {
            _contextManager = contextManager;
            _generateEmailWrapper = generateEmailWrapper;
        }

        public UserDto Authenticate(string email, string password)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var userByEmail = repo.GetByUserEmail(email);

            if (userByEmail.isSocialNetworks)
            {
                throw new ValidationException("You have already registered via Social Networks");
            }

            var user = repo.GetByUserEmailPassword(email, password);

            if (user == null)
            {
                throw new ValidationException("Incorrect Username/Password combination");
            }

            if (!user.IsVerified)
            {
                throw new ValidationException("Email not verified");
            }

            var result = MapToDto(user);
            return result;
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
                ValidatePassword(password);
                user.Password = password;
                _contextManager.Save();

                return "password is restored";
            }
            return "it is not possible to restore password";
        }

        public void ValidatePassword(string Password)
        {
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasNumber = new Regex(@"[0-9]+");

            if (!hasLowerChar.IsMatch(Password))
            {
                throw new ValidationException("Password should contain at least one lower case letter.");
            }
            else if (!hasUpperChar.IsMatch(Password))
            {
                throw new ValidationException("Password should contain at least one upper case letter.");
            }
            else if (Password.Length < 8)
            {
                throw new ValidationException("Password should contain at least 8 symbols.");
            }
            else if (!hasNumber.IsMatch(Password))
            {
                throw new ValidationException("Password should contain at least one numeric value.");
            }
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

        public bool SocialNetworksAuthenticate(string email, string name, string givenName, string familyName)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repo.GetByUserEmail(email);

            if (!(user == null))
            {
                return true;
            }
            else if (user == null)
            {
                new User(email, name, givenName, familyName);
            }

            return true;
        }
    }
}
