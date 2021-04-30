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
            var data = _service.GetCompany(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Companies()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateCompany([FromBody]CompanyDto model)
        {
            var data = _service.CreateCompany(model);
            return Ok(data);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateCompany([FromBody] CompanyDto model)
        {            
            var data = _service.UpdateCompany(model.Id,model);
            return Ok(data);
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
