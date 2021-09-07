
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Models.APIModels;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Study.EventManager.Services.Exceptions.ValidationException;

namespace Study.EventManager.Services
{
    internal class CompanyService : ICompanyService
    {
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;
        private IUploadService _uploadService;
        private readonly IMapper _mapper;
        private readonly string _urlAdress;
        private IEmailWrapper _emailWrapper;

        const string secretKey = "JoinCompanyViaLinkHash";

        public CompanyService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IUploadService uploadService, Settings settings
            , IEmailWrapper emailWrapper, IMapper mapper)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
            _uploadService = uploadService;
            _urlAdress = settings.FrontUrl;
            _emailWrapper = emailWrapper;
            _mapper = mapper;
        }

        public CompanyDto GetCompany(int id)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var comp = repo.GetById(id);
            var result = _mapper.Map<CompanyDto>(comp);
            return result;
        }

        public CompanyDto CreateCompany(CompanyCreateDto dto)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetById(dto.UserId);

            var repoCompany = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repoCompany.GetCompanyByName(dto.Name);

            if (!(company == null))
            {
                throw new ValidationException("Company with name <" + dto.Name + "> is already exists.");
            }

            var entity = _mapper.Map<Company>(dto);            
            repoCompany.Add(entity);

            _contextManager.Save();
                                    
            var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
            var companyUser = new CompanyUserLink
            {
                CompanyId = entity.Id,
                UserId = user.Id,
                UserCompanyRole = 1
            };
            repoCompUser.Add(companyUser);
            _contextManager.Save();

            return _mapper.Map<CompanyDto>(entity);
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

            return _mapper.Map<CompanyDto>(data); 
        }

        public CompanyDto MakeCompanyDel(int id, CompanyDto dto)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            data.Del = dto.Del;
            _contextManager.Save();

            return _mapper.Map<CompanyDto>(data);
        }

        public void DeleteCompany(int id)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var data = repo.GetById(id);
            repo.Delete(data);
            _contextManager.Save();
        }

        public PagedCompaniesDto GetAllByOwner(int userId, int page, int pageSize)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();            

            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetById(userId);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }
            var comp = repo.GetAllCompaniesByOwner(user.Id, page, pageSize);

            var companyDto = _mapper.Map<List<CompanyDto>>(comp);

            var retDto = new PagedCompaniesDto()
            {
                Companies = companyDto,
                Paging = new PagingDto()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = repo.GetAllCompaniesByOwnerCount(user.Id)
                }
            };
            return retDto;                       
        }

        public PagedCompaniesDto GetAllByUser(int userId, int page, int pageSize)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetById(userId);

            var repoUserCompanies = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
            var listUserCompanies = repoUserCompanies.GetCompaniesByUser(user.Id, page, pageSize);

            var companyListDto = _mapper.Map<List<CompanyDto>>(listUserCompanies);
            var companyIdList = companyListDto.Select(c => c.Id).ToList();
            var companyLinks = repoUserCompanies.GetCompanyUserLinkListForUser(userId, companyIdList);

            foreach (var oneCompany in companyListDto)
            {
                var thisCompanyLink = companyLinks.Where(c => c.CompanyId == oneCompany.Id).FirstOrDefault();
                if (thisCompanyLink != null)
                {
                    oneCompany.UserRole = thisCompanyLink.UserCompanyRole;
                }
            }

            var retDto = new PagedCompaniesDto()
            {
                Companies = companyListDto,
                Paging = new PagingDto()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = repoUserCompanies.GetCompaniesByUserCount(user.Id)
                }
            };

            return retDto;
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
                UserCompanyRole = 3
            };
            repoCompUser.Add(entity);

            _contextManager.Save();
            return "You successfully join the Company";
        }

        public async Task<CompanyDto> UploadCompanyFoto(int CompanyId, FileDto model)
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
            var serverFileName = "userId-" + company.Id.ToString() + "-" + guidStr;

            model.ServerFileName = serverFileName;
            var filePath = await _uploadService.Upload(model);
            company.OriginalFileName = model.File.FileName;
            company.FotoUrl = filePath.Url;
            company.ServerFileName = filePath.ServerFileName + model.File.FileName;

            _contextManager.Save();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> DeleteCompanyFoto(int CompanyId)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repo.GetById(CompanyId);

            if (company == null)
            {
                throw new ValidationException("Company not found");
            }

            await _uploadService.Delete(company.ServerFileName, "userfotoscontainer");
            company.ServerFileName = null;
            company.FotoUrl = null;
            company.OriginalFileName = null;
            _contextManager.Save();
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public string GenerateLinkToJoin(int CompanyId, DateTime date)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repo.GetById(CompanyId);

            if (company == null)
            {
                throw new ValidationException("Company not found");
            }
            
            string hashUrl = GetHashString(secretKey + company.Name);  
            
            hashUrl = System.Web.HttpUtility.UrlEncode(hashUrl);
            
            string url;
            if (!(date == DateTime.MinValue))
            {
                var dateStr = date.ToString("dd.MM.yyyy");
                dateStr = System.Web.HttpUtility.UrlEncode(dateStr);
                url = "?" + "companyid=" + company.Id + "&validTo=" + dateStr + "&code={" + hashUrl + "}";
            }
            else
            {
                url = "?" + "companyid=" + company.Id + "&code={" + hashUrl + "}";
            }
            
            url = _urlAdress + url;
            return url;
        }
  
        public string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        public string JoinCompanyViaLink(int CompanyId, string email, string Code)
        {
            var repo = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repo.GetById(CompanyId);
            if (company == null)
            {
                throw new ValidationException("Company not found");
            }

            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(email);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            string hashUrl = "{" + GetHashString(secretKey + company.Name) + "}";

            if (Code == hashUrl)
            {               
                var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
                var companyUser = repoCompUser.GetRecordByCompanyAndUser(user.Id, CompanyId);

                if (!(companyUser == null))
                {
                    throw new ValidationException("User is already added to the company.");
                }

                var entity = new CompanyUserLink
                {
                    CompanyId = CompanyId,
                    UserId = user.Id,
                    UserCompanyRole = 3
                };
                repoCompUser.Add(entity);
                _contextManager.Save();

                return "You are join the company";
            }
            else
            {
                return "Sorry, unexpected error";
            }            
        }

        public void InviteUsersToCompany(CompanyTreatmentUsersModel model)
        {          
            var company = GetCompany(model.CompanyId);
            if (company == null)
            {
                throw new ValidationException("Company not found.");
            }

            var generateEmail = new GenerateEmailDto
            {
                UrlAdress = _urlAdress + "/company/" + model.CompanyId + "?",
                EmailMainText = "Invitation to the company, for confirmation follow the link",
                ObjectId = model.CompanyId,
                Subject = "Welcome to the Company"
            };
                     
            foreach (string email in model.Email)
            {
                EmailFunctionality(email, model.CompanyId, generateEmail);
            }         
        }

        public string AppointUserAsAdmin(CompanyTreatmentUsersModel model)
        {
            var company = GetCompany(model.CompanyId);
            if (company == null)
            {
                throw new ValidationException("Company not found.");
            }

            var userError = "Something wrong with this email(s):";
            foreach (string email in model.Email)
            {
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetUserByEmail(email);
                if (!(user == null))
                {
                    var repo = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
                    var companyUser = repo.GetRecordByCompanyAndUser(user.Id, model.CompanyId);                    

                    if (!(companyUser == null))
                    {
                        companyUser.UserCompanyRole = 2;
                        _contextManager.Save();

                        var generateEmail = new GenerateEmailDto
                        {
                            UrlAdress = _urlAdress + "/company/" + model.CompanyId + "?",
                            EmailMainText = "Congratulations, you are Admin of " + company.Name +"! Now you can invite people to the company and create events.",
                            ObjectId = model.CompanyId,
                            Subject = "Company"
                        };                        
                        var emailModel = _generateEmailWrapper.GenerateEmail(generateEmail, user);
                        _emailWrapper.SendEmail(emailModel);
                    }
                    else
                    {
                        userError = userError + " " + email;
                    }
                }
                else
                {
                    userError = userError + " " + email;
                }                
            }
            if (userError == "Something wrong with this email:")
            {
                return "Users are successfully assigned";
            }
            else
            {
                return userError;
            }
        }
        
        public void AddUsersCSV(int CompanyId, IFormFile file)
        {
            var listEmails = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {                
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    listEmails.Add(csv.GetField("Email"));                             
                }       
            }
            var a = listEmails;

            var generateEmail = new GenerateEmailDto
            {
                UrlAdress = _urlAdress + "/company/" + CompanyId + "?",
                EmailMainText = "Invitation to the company, for confirmation follow the link",
                ObjectId = CompanyId,
                Subject = "Welcome to the Company"
            };

            foreach (string email in listEmails)
            {
                EmailFunctionality(email, CompanyId, generateEmail);
            }
        }

        public void EmailFunctionality(string email, int CompanyId, GenerateEmailDto dto)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FirstName = "",
                    LastName = "",
                    Username = ""
                };

                Thread thread = new Thread(() => _generateEmailWrapper.GenerateAndSendEmail(dto, user));
                thread.Start();
            }
            else
            {
                var repo = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
                var companyUser = repo.GetRecordByCompanyAndUser(user.Id, CompanyId);

                if (companyUser == null)
                {
                    Thread thread = new Thread(() => _generateEmailWrapper.GenerateAndSendEmail(dto, user));
                    thread.Start();
                }
            }

        }

        public PagedEventsDto GetCompanyEvents(int CompanyId, int page, int pageSize)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();

            var data = repo.GetAllEventsByCompanyId(CompanyId, page, pageSize);
            var eventDto = _mapper.Map<List<EventDto>>(data);

            var retDto = new PagedEventsDto()
            {
                Events = eventDto,
                Paging = new PagingDto()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = repo.GetAllEventsByCompanyIdCount(CompanyId)
                }
            };
            return retDto;
        }

        public void DeleteCompanyMember(int companyId, int userId)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetById(userId);

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

            var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
            var companyUser = repoCompUser.GetRecordByCompanyAndUser(userId, companyId);

            if (!(companyUser == null))
            {
                repoCompUser.Delete(companyUser);
                _contextManager.Save();
            }            
        }

        public void DemoteAdminToUser(int companyId, int userId)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetById(userId);

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

            var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
            var companyUser = repoCompUser.GetRecordByCompanyAndUser(userId, companyId);

            if (!(companyUser == null))
            {
                companyUser.UserCompanyRole = 3;
                _contextManager.Save();
            }
        }
    }
} 
