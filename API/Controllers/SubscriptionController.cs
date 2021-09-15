using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/SubscriptionController")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionService _service;

        public SubscriptionController(ISubscriptionService service)
        {
            _service = service;
        }
     
        [HttpGet]
        [Route("allSubscriptions")]
        public IActionResult AllSubscriptions()
        {
            try
            {
                var data = _service.GetAllSubscriptions();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateSub([FromBody] SubscriptionCreateDto dto)
        {
            try
            {               
                var data = _service.AddNewSubscription(dto);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(int subId, IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileDto = new FileDto
                    {
                        File = file,
                        Container = "subscriptionfotoscontainer"
                    };

                    return Ok(await _service.UploadSubFoto(subId, fileDto));
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
        public async Task<IActionResult> DeleteSubFoto(int subId)
        {
            try
            {
                var sub = await _service.DeleteSubscriptionFoto(subId);
                return Ok(sub);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateSubscription(int subId, [FromBody] SubscriptionRatesDto model)
        {
            try
            {              
                var data = _service.UpdateSubscription(subId, model);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteSubscription(int subId)
        {
            _service.DeleteSubscription(subId);
            return Ok("successful subscription delete");
        }

        [HttpPut]
        [Route("promoteSubscription")]
        public IActionResult PromoteSubscription(int subId, int companyId)
        {
            try
            {
                var data = _service.PromoteSubscription(subId, companyId);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
