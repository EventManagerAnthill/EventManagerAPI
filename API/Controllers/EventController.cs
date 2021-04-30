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
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService _service;

        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEvent(int id)
        {
            var data = _service.GetEvent(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Events()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateEvent([FromBody] EventDto model)
        {
            var data = _service.CreateEvent(model);
            return Ok(data);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateEvent([FromBody] EventDto model)
        {
            var data = _service.UpdateEvent(model.Id, model);
            return Ok(data);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            _service.DeleteEvent(id);
            return Ok("successful event delete");
        }
    }
}
