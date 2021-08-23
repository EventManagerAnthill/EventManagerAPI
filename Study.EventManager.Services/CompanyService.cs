using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study.EventManager.Services
{
    internal class CompanyService : ICompanyService
    {        
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;
        private IEnumerable<Company> data;
        private IUploadService _uploadService;
        private readonly string _urlAdress;

        public CompanyService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IUploadService uploadService, Settings settings)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
            _uploadService = uploadService;
            _urlAdress = settings.FrontUrl;
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
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(dto.Email);

            var repoCompany = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repoCompany.GetCompanyByName(dto.Name);

            if (!(company == null))
            {
                throw new ValidationException("Company with name <" + dto.Name + "> is already exists.");
            }

            var entity = MapToEntity(dto, user.Id);
            repoCompany.Add(entity);

            _contextManager.Save();
            user.Companies.Add(entity);

            _contextManager.Save();

            //second variant
            var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
            var companyUser = new CompanyUserLink
            {
                CompanyId = entity.Id,
                UserId = user.Id,
                UserRole = 1
            };
            repoCompUser.Add(companyUser);
            _contextManager.Save();

            return MapToDto(entity);          
        }

        public CompanyDto UpdateCompany(int id, CompanyDto dto)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repo.GetCompanyByName(dto.Name);

            if (!(company == null))
            {
                if ((company.Name == dto.Name) && !(company.Id == dto.Id))
                {
                    throw new ValidationException("Company with name <" + dto.Name + "> is already exists.");
                } 
            }

            var data = repo.GetById(id);
            if (data == null)
            {
                throw new ValidationException("Company not found.");
            }

            data.Name = dto.Name;
            data.Type = dto.Type;
            data.Description = dto.Description;

            _contextManager.Save();

            return MapToDto(data);
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

        public IEnumerable<CompanyDto> GetAllByOwner(string email)
        {
            try
            {
                var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
                
                if (!(email == null))
                {
                    var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                    var user = repoUser.GetUserByEmail(email);

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
                throw new ValidationException("Sorry, unexpected error.");
            }
        }

        public List<Company> GetAllByUser(string email)
        {
            try
            {
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetUserByEmail(email);

                var listUserCompanies = repoUser.GetCompaniesByUser(user.Id);

                return listUserCompanies;
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
            }
        }

        public int CountCompanyUser(int companyId)
        {
            try
            {
                var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
                var cnt = repo.GetCompanyCountUsers(companyId);
                return cnt;
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
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
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);

            if (user == null)
            {
                throw new ValidationException("Incorrect email combination");
            }

            var company = GetCompany(companyId);
            if (!(company == null))
            {                
                throw new ValidationException("Company not found.");
            }

            var userCompanies = repoUser.GetCompaniesByUser(user.Id);
            if (userCompanies.Any(x => x.Id == companyId))
            {
                throw new ValidationException("User with this email is already exist in company " + company.Name);
            }

            var generateEmail = new GenerateEmailDto
            {
                //UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/company/" + companyId + "?",

                UrlAdress = _urlAdress + "/company/" + companyId + "?",
                EmailMainText = "Invitation to the company, for confirmation follow the link",
                ObjectId = companyId,
                Subject = "Welcome to the Company"
            };

            _generateEmailWrapper.GenerateEmail(generateEmail, user);          
        }

        public string AcceptInvitation(int companyId, string Email)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);         
            
            if (user == null)
            {
                throw new ValidationException("User not found.");
            }
                     
            var repoCompany = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repoCompany.GetById(companyId);

            if (company == null)
            {
                throw new ValidationException("Company not found.");
            }

            //first variant
            var userCompanies = repoUser.GetCompaniesByUser(user.Id);
            if (userCompanies.Any(x => x.Id == companyId))
            {
                throw new ValidationException("User is already added to the company.");
            }
            user.Companies.Add(company);
            //end first variant

            //second variant
            var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
            var companyUser = repoCompUser.GetRecordByCompanyAndUser(user.Id, companyId);
            
            if (!(companyUser == null))
            {
                throw new ValidationException("User is already added to the company.");
            }

            var entity = new CompanyUserLink
            {
                CompanyId = companyId,
                UserId = user.Id,
                UserRole = 3
            };
            repoCompUser.Add(entity);
            //end second variant

            _contextManager.Save();
            return "You successfully join the Company";
        }

        public async Task UploadCompanyFoto(int CompanyId, FileDto model)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repo.GetById(CompanyId);

            if (company == null)
            {
                throw new ValidationException("Company not found");
            }

            if (!(company.ServerFileName == null))
            {
                await _uploadService.Delete(company.ServerFileName, model.Container);
            }

            var guidStr = Guid.NewGuid().ToString();
            var serverFileName = "companyId-" + company.Id.ToString() + "-" + guidStr;

            model.ServerFileName = serverFileName;
            var filePath = await _uploadService.Upload(model);
            company.OriginalFileName = model.ImageFile.FileName;
            company.FotoUrl = filePath.Url;
            company.ServerFileName = filePath.ServerFileName + model.ImageFile.FileName;

            _contextManager.Save();
        }

        public async Task DeleteCompanyFoto(int CompanyId)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repo.GetById(CompanyId);

            if (company == null)
            {
                throw new ValidationException("Company not found");
            }

            await _uploadService.Delete(company.ServerFileName, "companyfotos");
            company.ServerFileName = null;
            company.FotoUrl = null;
            company.OriginalFileName = null;
            _contextManager.Save();
        }
    }
} 
