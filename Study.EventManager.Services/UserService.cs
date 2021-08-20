using Azure.Storage.Blobs;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Study.EventManager.Services
{
    internal class UserService : IUserService
    {
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;
        private IUploadService _uploadService;
         
        public UserService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IUploadService uploadService)
        {         
            _generateEmailWrapper = generateEmailWrapper;            
            _contextManager = contextManager;
            _uploadService = uploadService;
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
            var repo = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repo.GetUserByEmail(dto.Email);

            if (!(user == null))
            {
                throw new ValidationException("User with email address <" + dto.Email + "> is already exists.");
            }

            ValidateUser(dto.FirstName, dto.LastName, dto.Email);
            ValidatePassword(dto.Password);
            SendWelcomeEmail(dto);           
          
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
        
        public void ValidateUser(string FirstName, string LastName, string Email)
        {
            if (Email == null)
            {
                throw new ValidationException("Email is incorect");
            }

            var validEmail = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (!Regex.IsMatch(Email, validEmail, RegexOptions.IgnoreCase))
            {
                throw new ValidationException("Email is incorect");
            }

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                throw new ValidationException("FirstName or LastName incorect");
            }
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
                    UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/signin?",
                    EmailMainText = "You are currently registered using",
                    ObjectId = 0,
                    Subject = "Welcome"
                };

                _generateEmailWrapper.GenerateEmail(generateEmail, user);
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
            }
        }

        public async Task UploadUserFoto(FileDto model)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(model.Email);
          
            if (user == null)
            {
                throw new ValidationException("User not found"); 
            }

            if (!(user.ServerFileName == null))
            {
                await _uploadService.Delete(user.ServerFileName);
            }

            var guidStr = Guid.NewGuid().ToString();
            var serverFileName = "userId-" + user.Id.ToString() + "-" + guidStr;

            model.ServerFileName = serverFileName;
            var filePath = await _uploadService.Upload(model);
            user.OriginalFileName = model.ImageFile.FileName;
            user.FotoUrl = filePath.Url;
            user.ServerFileName = filePath.ServerFileName + model.ImageFile.FileName;

            _contextManager.Save();         
        }

        public async Task DeleteUserFoto(string email)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(email);

            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            await _uploadService.Delete(user.ServerFileName, "userfotos");
            user.ServerFileName = null;
            user.FotoUrl = null;
            user.OriginalFileName = null;
            _contextManager.Save();
        }
    }
}