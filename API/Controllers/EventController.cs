using API.Exceptions;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System.ComponentModel.DataAnnotations;

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
        public IActionResult SendInviteEmail()
        {
            try
            {
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
                return Ok(response);
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
                var data = _service.GetEventByUserId(id);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        [HttpPost]
        [Route("")]
        public IActionResult CreateEvent([FromBody] EventCreateModel model)
        {
            try
            {
                var eventCreateDto = new EventCreateDto
                {
                    Name = model.Name,
                    UserId = model.UserId,
                    Type = model.Type,
                    Description = model.Description,
                    CreateDate = model.CreateDate,
                    HoldingDate = model.HoldingDate,
                    CompanyId = model.CompanyId
                    
                };
                var data = _service.CreateEvent(eventCreateDto);
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

        [HttpPut]
        [Route("MakeEventDel/{id}")]
        public IActionResult MakeEvenDel(int id)
        {
            try
            {
                var eventDto = new EventDto
                {
                    Id = id,
                    Del = 1
                };
                var data = _service.MakeEventDel(eventDto.Id, eventDto);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("CancelEvent/{id}")]
        public IActionResult CancelEvent(int id)
        {
            try
            {
                var eventDto = new EventDto
                {
                    Id = id,
                    Status = 1
                };
                var data = _service.CancelEvent(eventDto.Id, eventDto);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllEventsByUser")]
        public IActionResult CompaniesByUser(string email)
        {
            try
            {
                var data = _service.GetAllByUser(email);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("eventReview")]
        public IActionResult EventReview(EventReviewCreateDto dto)
        {
            try
            {                
                var data = _service.EventReview(dto);
                return Ok(data);
            }
            catch (APIEventException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
