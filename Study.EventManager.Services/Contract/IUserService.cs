using Study.EventManager.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface IUserService
    {        
        UserDto GetUser(string email);
        UserDto CreateUser(UserCreateDto dto);
        UserDto UpdateUser(int id, UserDto model);
        IEnumerable<UserDto> GetAll();
        void DeleteUser(int id);
        Task<UserDto> UploadUserFoto(string email, FileDto model);
        Task<UserDto> DeleteUserFoto(string email);
        UserDto UpdatePasswordUser(UserUpdatePasswordDto dto);
    }
}
