using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
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

        [HttpPost]
        [Route("")]
        public IActionResult CreateEvent([FromBody] UserDto model)
        {
            var data = _service.CreateUser(model);
            return Ok(data);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateEvent([FromBody] UserDto model)
        {
            var data = _service.UpdateUser(model.Id, model);
            return Ok(data);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            _service.DeleteUser(id);
            return Ok("successful user delete");
        }
    }
}