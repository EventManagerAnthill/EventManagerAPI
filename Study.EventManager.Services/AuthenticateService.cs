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
        private readonly string _urlAdress;
        private IEmailWrapper _emailWrapper;

        public AuthenticateService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, Settings settings, IEmailWrapper emailWrapper)
        {
            _contextManager = contextManager;
            _generateEmailWrapper = generateEmailWrapper;
            _urlAdress = settings.FrontUrl;
            _emailWrapper = emailWrapper;
        }

        public UserDto Authenticate(string email, string password)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var userByEmail = repo.GetUserByEmail(email);

            if (!(userByEmail == null))
            {           
                if (userByEmail.isSocialNetworks)
                {
                    throw new ValidationException("You have already registered via Social Networks");
                }
            }

            var user = repo.GetUserByEmailPassword(email, password);

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
                var user = repo.GetUserByEmail(email);                

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
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repo.GetUserByEmail(email);

            if (user == null)
            {
                throw new ValidationException("Incorrect email combination");
            }

            if (user.isSocialNetworks)
            {
                throw new ValidationException("You have already registered via Social Networks");
            }

            var generateEmail = new GenerateEmailDto
            {             
                UrlAdress = _urlAdress + "/resetpassword?",
                EmailMainText = "Password recovery",
                ObjectId = 0,
                Subject = "Password recovery"
            };

            var emailModel = _generateEmailWrapper.GenerateEmail(generateEmail, user);
            _emailWrapper.SendEmail(emailModel);
        }

        public string VerifyUrlEmail(string email, string code)
        {
            string hashUrl = "{" + _generateEmailWrapper.GetHashString(secretKey + email) + "}";

            if (code == hashUrl)
            {
                var repo = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repo.GetUserByEmail(email);
                user.IsVerified = true;
                _contextManager.Save();

                return "email is verified";
            }
            return "it is not possible to confirm email";
        }

        public bool SocialNetworksAuthenticate(string email, string givenName, string familyName, string url)
        {
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repo.GetUserByEmail(email);

            if (!(user == null))
            {
                return true;
            }
            else if (user == null)
            {
                User entity = new User(email, givenName, familyName, url);
                repo.Add(entity);
                _contextManager.Save();
            }
           
            return true;
        }

    }
}
