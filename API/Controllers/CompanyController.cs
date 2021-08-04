using API.Exceptions;
using API.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCompany(int id)
        {
            try
            {
                var data = _service.GetCompany(id);
                return Ok(data);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sendInviteEmail")]
        public IActionResult SendInviteEmail(CompanyUserModel model)
        {
            try
            {
                _service.sendInviteEmail(model.CompanyId, model.Email);
                return Ok("link to join company is sent to specified  email");
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("acceptInvitation")]
        public IActionResult AcceptInvitation(int CompanyId, string Email)
        {
            try
            {
                var response = _service.AcceptInvitation(CompanyId, Email);
                return Ok(response);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Companies()
        {
            try
            {
                var data = _service.GetAll();
                return Ok(data);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllCompaniesByUser")]
        public IActionResult CompaniesByUser(string email)
        {
            try
            {
                var data = _service.GetAll(email);
                return Ok(data);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateCompany([FromBody]CompanyCreateModel model)
        {
            try
            {
                var companyCreateDto = new CompanyCreateDto
                {
                    Name = model.Name,
                    Email = model.Email,
                    Type = model.Type,
                    Description = model.Description

                };
                var data = _service.CreateCompany(companyCreateDto);
                return Ok(data);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateCompany(int id, [FromBody] CompanyUpdateModel model)
        {
            try
            {
                var userDto = new CompanyDto
                {
                    Id = id,
                    Name = model.Name,
                    Type = model.Type,
                    Description = model.Description

                };
                var data = _service.UpdateCompany(userDto.Id, userDto);
                return Ok(data);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("MakeCompanyDel/{id}")]
        public IActionResult MakeCompanyDel(int id)
        {
            try
            {
                var userDto = new CompanyDto
                {
                    Id = id,
                    Del = 1
                };
                var data = _service.MakeCompanyDel(userDto.Id, userDto);
                return Ok(data);
            }
            catch (APICompanyExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCompany(int id)
        {
            _service.DeleteCompany(id);
            return Ok("successful company delete");
        }
    }
}
