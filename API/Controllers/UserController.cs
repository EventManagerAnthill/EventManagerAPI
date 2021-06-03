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
                    emmail = user.Email
                };

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
            var data = _service.GetUser(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Users()
        {
            var data = _service.GetAll();
            return Ok(data);
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

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            _service.DeleteUser(id);
            return Ok("successful user delete");
        }
    }
}