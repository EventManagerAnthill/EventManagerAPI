using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Study.EventManager.Services
{
    public class CompanyService : ICompanyService
    {
        private IContextManager _contextManager;

        public CompanyService(IContextManager contextManager)
        {
            _contextManager = contextManager;
        }

        public CompanyDto GetCompany(int id)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            var result = MapToDto(data);
            return result;
        }

        public CompanyDto CreateCompany(CompanyDto dto)
        {
            var entity = MapToEntity(dto);
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            repo.Add(entity);
            _contextManager.Save();
            return MapToDto(entity);
        }

        public CompanyDto UpdateCompany(int id, CompanyDto dto)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            data.Name = dto.Name;
            _contextManager.Save();

            return MapToDto(data);
        }

        public void DeleteCompany(int id)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            var entity = repo.Delete(data);
            _contextManager.Save();          
        }

        public IEnumerable<CompanyDto> GetAll()
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetAll();
            return data.Select(x => MapToDto(x)).ToList();
        }

        private CompanyDto MapToDto(Company entity)
        {
            if (entity ==null)
            {
                return null;
            } 
            return new CompanyDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }  
        private Company MapToEntity(CompanyDto dto)
        {
            return new Company
            {   
                Name = dto.Name
            };
        }
    }
} 
