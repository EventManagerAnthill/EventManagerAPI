using API.Models;
using Microsoft.AspNetCore.Authorization;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Study.EventManager.Services.Dto;

namespace API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
  
    public class UserController : ControllerBase
    {
        private IUserService _serviceUser;

        public UserController(IUserService service)
        {
            _serviceUser = service;

        }                      

        [HttpGet]
        [Route("getUser")]
        public IActionResult GetUser(string email)
        {
            try
            {
                var data = _serviceUser.GetUser(email);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Users()
        {
            try
            {
                var data = _serviceUser.GetAll();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]

        [Route("")]
        public IActionResult CreateUser([FromBody] UserCreateModel model)
        {
            try
            {
                var userCreateDto = new UserCreateDto
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    EmailVerification = model.EmailVerification
                };

                var data = _serviceUser.CreateUser(userCreateDto);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateModel model)
        {
            try
            {
                var userDto = new UserDto
                {
                    Id = id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    BirthDate = model.BirthDate,
                    Phone = model.Phone,
                    Sex = model.Sex
                };

                var data = _serviceUser.UpdateUser(id, userDto);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _serviceUser.DeleteUser(id);
                return Ok("successful user delete");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(string email,  IFormFile file)//([FromForm] FileAPIModel model)
        {
            try
            {
                if (file.Length > 0)
                {                   
                    var fileDto = new FileDto
                    {
                        File = file,                
                        Container = "userfotoscontainer"
                    };

                    return Ok(await _serviceUser.UploadUserFoto(email, fileDto));  
                }
                else
                {
                    throw new ValidationException("foto not found");
                }        
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteFoto")]
        public async Task<IActionResult> DeleteUserFoto(string email)
        {
            try
            {
                var user = await _serviceUser.DeleteUserFoto(email);
                return Ok(user);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updatePassword")]
        public IActionResult UpdatePasswordUser([FromBody] UpdateUserPasswordModel model)
        {
            try
            {
                var userPassDto = new UserUpdatePasswordDto
                {
                    UserId = model.UserId,
                    Password = model.Password
                };

                var data = _serviceUser.UpdatePasswordUser(userPassDto);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


/*        [HttpGet]
        [Route("searchByName")]
        public IActionResult SearchByName()
        {
            try
            {
                var data = _serviceUser.GetAll();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }*/


    }    
}
