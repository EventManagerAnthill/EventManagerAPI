using API.Exceptions;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
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
            catch (ValidationException ex)
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("acceptInvitation")]
        public IActionResult AcceptInvitation(CompanyUserModel model)
        {
            try
            {
                var response = _service.AcceptInvitation(model.CompanyId, model.Email);
                return Ok(response);
            }
            catch (ValidationException ex)
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
                var data = _service.GetAllByOwner();
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllCompaniesByOwner")]
        public IActionResult CompaniesByOwner(string email)
        {
            try
            {
                var data = _service.GetAllByOwner(email);
                return Ok(data);
            }
            catch (ValidationException ex)
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
                var data = _service.GetAllByUser(email);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCompanyCountUsers")]
        public IActionResult GetCompanyCountUsers(int companyId)
        {
            try
            {
                var data = _service.CountCompanyUser(companyId);
                return Ok(data);
            }
            catch (ValidationException ex)
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
            catch (ValidationException ex)
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
            catch (ValidationException ex)
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
                var companyDto = new CompanyDto
                {
                    Id = id,
                    Del = 1
                };
                var data = _service.MakeCompanyDel(companyDto.Id, companyDto);
                return Ok(data);
            }
            catch (ValidationException ex)
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
                        Container = "companyfotos"
                    };

                    await _service.UploadCompanyFoto(id, fileDto);
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
        public async Task<IActionResult> DeleteCompanyFoto(UserEmailModel model)
        {
            try
            {
                //await _serviceUser.DeleteUserFoto(model.Email);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLinkToJoinCompany")]
        public IActionResult GetLinkToJoinCompany(int Id, DateTime date)
        {
            try
            {
                var data = _service.GenerateLinkToJoin(Id, date);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult JoinCompanyViaLink(JoinCompanyModel model)
        {
            try
            {                                  
                var result = _service.JoinCompanyViaLink(model.CompanyId, model.Email, model.Code);        
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

