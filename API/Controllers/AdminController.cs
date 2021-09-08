using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]   
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminService _serviceAdmin;

        public AdminController(IAdminService serviceAdmin)
        {
            _serviceAdmin = serviceAdmin;
        }
     
        [HttpGet]
        [Route("allCompanies")]
        public IActionResult Companies()
        {
            try
            {
                var data = _serviceAdmin.GetAllCompanies();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("allEvents")]
        public IActionResult Events()
        {
            try
            {
                var data = _serviceAdmin.GetAllEvents();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("allUsers")]
        public IActionResult Users()
        {
            try
            {
                var data = _serviceAdmin.GetAllUsers();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
