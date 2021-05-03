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
        private readonly AppSettings _appSettings;

        public UserService(IContextManager contextManager)
        {
            _contextManager = contextManager;
        }

        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        public AuthenticateResponseDto Authenticate(AuthenticateRequestDto model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponseDto(user, token);
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

            data.FirstName = dto.Surname;
            data.LastName = dto.Name;
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
                Surname = entity.FirstName,
                Name = entity.LastName,                
                Middlename = entity.Middlename,
                BirthDate = entity.BirthDate,
                Email = entity.Email,
                Phone = entity.Phone,
                Sex = entity.Sex,
                Username = entity.Username,
                Password = entity.Password
            };
        }
        private User MapToEntity(UserDto dto)
        {
            return new User
            {
                FirstName = dto.Surname,
                LastName = dto.Name,
                Middlename = dto.Middlename,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                Phone = dto.Phone,
                Sex = dto.Sex,
                Username = dto.Username,
                Password = dto.Password
            };
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
      
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
    }
}