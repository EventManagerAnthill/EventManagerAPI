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
        private readonly IUploadService _uploadService;

        public UserController(IUserService service, IUploadService uploadService)
        {
            _serviceUser = service;
            _uploadService = uploadService;
        }                      

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var data = _serviceUser.GetUser(id);
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
                    Middlename = model.Middlename,
                    BirthDate = model.BirthDate,
                    Phone = model.Phone,
                    Email = model.Username,
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

                    await _serviceUser.UploadUserFoto(email, fileDto);  
                }
                else
                {
                    throw new ValidationException("foto not found");
                }

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteFoto")]
        public async Task<IActionResult> DeleteUserFoto(UserEmailModel model)
        {
            try
            {
                await _serviceUser.DeleteUserFoto(model.Email);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }    
}
