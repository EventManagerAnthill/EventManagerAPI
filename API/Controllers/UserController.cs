using API.Middlewares.Autentication;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authentication.Facebook;
using Google.Apis.Auth;

namespace API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _serviceUser;

      //  private IAuthenticateService _serviceAuth;

      //  private AuthOptions _authOptions;

        public UserController(IUserService service, IConfiguration config)
        {
            _serviceUser = service;
           // _authOptions = config.GetSection("AuthOptions").Get<AuthOptions>();       
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

    }    
}
