using API.Middlewares.Autentication;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

namespace API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        private AuthOptions _authOptions;

        public UserController(IUserService service, IConfiguration config)
        {
            _service = service;
            _authOptions = config.GetSection("AuthOptions").Get<AuthOptions>();
        }

        private IEnumerable<Claim> GetClaims(UserDto user)
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier , user.LastName)
            };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequestModel model)
        {
            try
            {
                var user = _service.Authenticate(model.Email, model.Password);

                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                     issuer: _authOptions.Issuer,
                     audience: _authOptions.Audience,
                     notBefore: now,
                     claims: GetClaims(user),
                     expires: now.Add(TimeSpan.FromMinutes(_authOptions.LifeTime)),
                     signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(_authOptions.SecretKey), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    email = user.Email
                };

                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("validateUser")]
        public IActionResult ValidateUser(string email, string validTo, string code)
        {
            try
            {
                var date = DateTime.Today;

                var validDT = DateTime.ParseExact(validTo, "dd.MM.yyyy", null);

                if (validDT < date)
                {
                    return BadRequest("url expired");
                }

                var IsVerified = _service.VerifyUrlEmail(email, code);

                return Ok(IsVerified);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("sendRestoreEmail")]        
        public IActionResult SendRestoreEmail(UserRestoreEmailModel model)
        {
            try
            {
                _service.sendRestoreEmail(model.Email); 
                return Ok("link to restore your password sent to email");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("restorePassword")]
        public IActionResult restorePass(UserRestorePasswordModel model)
        {                       
            try
            {
                var date = DateTime.Today;
                var validDT = DateTime.ParseExact(model.validTo, "dd.MM.yyyy", null);

                if (validDT < date)
                {
                    return BadRequest("url expired");
                }

                var response = _service.restorePass(model.Email, model.Password, model.code);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var data = _service.GetUser(id);
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
                var data = _service.GetAll();
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

                var data = _service.CreateUser(userCreateDto);
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

                var data = _service.UpdateUser(id, userDto);
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
                _service.DeleteUser(id);
                return Ok("successful user delete");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
