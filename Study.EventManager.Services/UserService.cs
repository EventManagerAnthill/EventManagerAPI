using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Study.EventManager.Services.Dto
{
    public class UserService : IUserService
    {

        private IContextManager _contextManager;

        public UserService(IContextManager contextManager)
        {
            _contextManager = contextManager;
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

            data.Surname = dto.Surname;
            data.Name = dto.Name;
            data.Patron = dto.Patron;
            data.Birth = dto.Birth;
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
                Surname = entity.Surname,
                Name = entity.Name,                
                Patron = entity.Patron,
                Birth = entity.Birth,
                Email = entity.Email,
                Phone = entity.Phone,
                Sex = entity.Sex
        };
        }
        private User MapToEntity(UserDto dto)
        {
            return new User
            {
                Surname = dto.Surname,
                Name = dto.Name,
                Patron = dto.Patron,
                Birth = dto.Birth,
                Email = dto.Email,
                Phone = dto.Phone,
                Sex = dto.Sex
            };
        }
    }
}