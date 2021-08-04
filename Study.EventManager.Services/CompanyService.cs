using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Study.EventManager.Services
{
    internal class CompanyService : ICompanyService
    {        
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;
        private IEnumerable<Company> data;

        public CompanyService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
        }

        public CompanyDto GetCompany(int id)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            var result = MapToDto(data);
            return result;
        }

        public CompanyDto CreateCompany(CompanyCreateDto dto)
        {
            try
            {
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetByUserEmail(dto.Email);

                var repoCompany = _contextManager.CreateRepositiry<ICompanyRepo>();

                var company = repoCompany.GetCompanyByName(dto.Name);

                if (!(company == null))
                {
                    throw new ValidationException("Company with name <" + dto.Name + "> is already exists.");
                }

                var entity = MapToEntity(dto, user.Id);
                repoCompany.Add(entity);
                _contextManager.Save();
                return MapToDto(entity);
            }
            catch
            {
                throw new CompanyExceptions("Sorry, unexpected error.");
            }
        }

        public CompanyDto UpdateCompany(int id, CompanyDto dto)
        {
            try
            {
                var repo = _contextManager.CreateRepositiry<ICompanyRepo>();

                var company = repo.GetCompanyByName(dto.Name);

                if (!(company == null))
                {
                    throw new ValidationException("Company with name <" + dto.Name + "> is already exists.");
                }
                var data = repo.GetById(id);
                data.Name = dto.Name;
                _contextManager.Save();

                return MapToDto(data);
            }
            catch
            {
                throw new CompanyExceptions("Sorry, unexpected error.");
            }
        }

        public CompanyDto MakeCompanyDel(int id, CompanyDto dto)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            data.Del = dto.Del;
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

        public IEnumerable<CompanyDto> GetAll(string email)
        {
            try
            {
                var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
                
                if (!(email == null))
                {
                    var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                    var user = repoUser.GetByUserEmail(email);

                    data = repo.GetAll().Where(x => x.Del == 0 && x.UserId == user.Id);
                }
                else
                {
                    data = repo.GetAll().Where(x => x.Del == 0);
                }
              
                return data.Select(x => MapToDto(x)).ToList();
            }
            catch
            {
                throw new CompanyExceptions("Sorry, unexpected error.");
            }
        }

        private CompanyDto MapToDto(Company entity)
        {
            if (entity == null)
            {
                return null;
            } 
            return new CompanyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UserId = entity.UserId,
                Type = entity.Type,
                Description = entity.Description,
                Del = entity.Del
            };
        }  

        private Company MapToEntity(CompanyCreateDto dto, int userId)
        {
            return new Company
            {   
                Name = dto.Name,
                UserId = userId,
                Type = dto.Type,
                Description = dto.Description
            };
        }

        public void sendInviteEmail(int companyId, string Email)
        {
            try
            {             
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetByUserEmail(Email);

                if (user == null)
                {
                    throw new ValidationException("Incorrect email combination");
                }

                var repoCompanyUser = _contextManager.CreateRepositiry<ICompanyUserRepo>();
                var companyUser = repoCompanyUser.GetCompanyUser(companyId, user.Id);

                if (!(companyUser == null))
                {
                    var company = GetCompany(companyId);
                    throw new ValidationException("User with this email is already exist in company " + company.Name);
                }

                var generateEmail = new GenerateEmailDto
                {
                    UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/company/"+ companyId + "?",
                    EmailMainText = "Invitation to the company, for confirmation follow the link",
                    ObjectId = companyId
                };

                _generateEmailWrapper.GenerateEmail(generateEmail, user);
            }
            catch
            {
                throw new CompanyExceptions("Sorry, unexpected error.");
            }
        }

        public string AcceptInvitation(int companyId, string Email)
        {
            try
            {
                var repoCompanyUser = _contextManager.CreateRepositiry<ICompanyUserRepo>();

                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetByUserEmail(Email);

                var entity = new CompanyUser
                {
                    CompanyId = companyId,
                    UserId = user.Id
                };

                var companyUser = repoCompanyUser.Add(entity);
                return "You successfully join the Company";
            }
            catch
            {
                throw new CompanyExceptions("Sorry, unexpected error.");
            }
        }


    }
} 
