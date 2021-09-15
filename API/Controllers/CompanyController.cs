using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Models.APIModels;
using System;
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
        public IActionResult GetCompany(int id, int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return BadRequest("User not found");
                }
                var data = _service.GetCompany(id, userId);
                return Ok(data);
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
        [Route("getAllCompaniesByOwner")]
        public IActionResult CompaniesByOwner(int userId, int page = 1, int pageSize = 10, string companyName = "")
        {
            try
            {
                var data = _service.GetAllByOwner(userId, page, pageSize, companyName);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllCompaniesByUser")]
        public IActionResult CompaniesByUser(int userId, int page = 1, int pageSize = 10, string companyName = "")
        {
            try
            {
                var data = _service.GetAllByUser(userId, page, pageSize, companyName);
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
                    UserId = model.UserId,
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
        [Route("makeCompanyDel/{id}")]
        public IActionResult MakeCompanyDel(int id)
        {
            try
            {
                var data = _service.MakeCompanyDel(id);
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
        public async Task<IActionResult> Upload(int companyId, IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {                    
                    var fileDto = new FileDto
                    {
                        File = file,                   
                        Container = "companyfotoscontainer"
                    };

                    return Ok(await _service.UploadCompanyFoto(companyId, fileDto));
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
        public async Task<IActionResult> DeleteCompanyFoto(int CompanyId)
        {
            try
            {
                var company = await _service.DeleteCompanyFoto(CompanyId);
                return Ok(company);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getLinkToJoinCompany")]
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
        [Route("joinCompanyViaLink")]
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

        [HttpPost]
        [Route("inviteUsers")]
        public IActionResult InviteEmail(CompanyTreatmentUsersModel model)
        {
            try
            {
                _service.InviteUsersToCompany(model);
                return Ok("Link to join the company is sent to emails");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("appointUserAsAdmin")]
        public IActionResult AppointUserAsAdmin(CompanyTreatmentUsersModel model)
        {
            try
            {
                var result = _service.AppointUserAsAdmin(model);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("demoteAdminToUser")]
        public IActionResult DemoteAdminToUser(int companyId, int userId)
        {
            try
            {
                _service.DemoteAdminToUser(companyId, userId);
                return Ok("Admin successfully demote");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("addUsersCSV")]
        public async Task<IActionResult> AddUsersCSV(int Companyid, IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    _service.AddUsersCSV(Companyid, file);
                }
                else
                {
                    throw new ValidationException("file not found");
                }

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getCompanyEvents")]
        public IActionResult GetCompanyEvents(int CompanyId, int page = 1, int pageSize = 10)
        {
            try
            {
                var data = _service.GetCompanyEvents(CompanyId, page, pageSize);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getCompanyUsers")]
        public IActionResult GetCompanyUsers(int CompanyId, int page = 1, int pageSize = 10, string firstName = "", string lastName = "")
        {
            try
            {
                var data = _service.GetCompanyUsers(CompanyId, page, pageSize, firstName, lastName);
                return Ok(data);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteMember")]
        public IActionResult DeleteCompanyMember(int companyId, int userId)
        {
            try
            {
                 _service.DeleteCompanyMember(companyId, userId);
                return Ok("User deleted successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

