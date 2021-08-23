using API.Exceptions;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost("sendInviteEmail")]
        public IActionResult SendInviteEmail(EventUserModel model)
        {
            try
            {
                _service.sendInviteEmail(model.EventId, model.Email);
                return Ok("link to join event is sent to specified  email");
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("acceptInvitation")]
        public IActionResult AcceptInvitation(int EventId, string Email)
        {
            try
            {
                var response = _service.AcceptInvitation(EventId, Email);
                return Ok(1);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEvent(int id)
        {
            try
            {
                var data = _service.GetEvent(id);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Events()
        {
            try
            {
                var data = _service.GetAll();
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEventsByUserId")]
        public IActionResult GetEventsByUserId(int id)
        {
            try
            {
                var data = _service.GetEventsByUserId(id);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        [HttpPost]
        [Route("")]
        public IActionResult CreateEvent([FromBody] EventDto model)
        {
            try
            {
                var data = _service.CreateEvent(model);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateEvent([FromBody] EventDto model)
        {
            try
            {
                var data = _service.UpdateEvent(model.Id, model);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            try
            {
                _service.DeleteEvent(id);
                return Ok("successful event delete");
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Comment.
        /// </summary>
        [HttpPut]
        [Route("MakeEventDel/{id}")]
        public IActionResult MakeEvenDel(int id)
        {
            try
            {
                var userDto = new EventDto
                {
                    Id = id,
                    Del = 1
                };
                var data = _service.MakeEventDel(userDto.Id, userDto);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(int id, IFormFile file)//([FromForm] FileAPIModel model)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileDto = new FileDto
                    {
                        ImageFile = file,
                        Container = "eventfotos"
                    };

                    await _service.UploadEventFoto(id, fileDto);
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
        public async Task<IActionResult> DeleteEventFoto(int EventId)
        {
            try
            {
                await _service.DeleteEventFoto(EventId);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
