
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
        private IEnumerable<Company> data;
        private IUploadService _uploadService;
        private readonly string _urlAdress;
        private IEmailWrapper _emailWrapper;

        const string secretKey = "JoinCompanyViaLinkHash";

        public CompanyService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IUploadService uploadService, Settings settings, IEmailWrapper emailWrapper)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
            _uploadService = uploadService;
            _urlAdress = settings.FrontUrl;
            _emailWrapper = emailWrapper;
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

    /*    public void sendInviteEmail(int companyId, string Email)
        {           
            var company = GetCompany(companyId);
            if (company == null)
            {
                throw new ValidationException("Company not found.");
            }

            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);

            if (user == null)
            {
                user = new User
                {
                    Email = Email,
                    FirstName = "Dear",
                    LastName = "Anonym",
                    Username = "Anonym"
                }; 
            }
            else
            {
                var repo = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
                var companyUser = repo.GetRecordByCompanyAndUser(user.Id, companyId);

                if (companyUser == null)
                {
                    throw new ValidationException("User with this email is already exist in company " + company.Name);
                }               
            }

            var generateEmail = new GenerateEmailDto
            {
                //UrlAdress = "https://steventmanagerdev01.z13.web.core.windows.net/company/" + companyId + "?",

                UrlAdress = _urlAdress + "/company/" + companyId + "?",
                EmailMainText = "Invitation to the company, for confirmation follow the link",
                ObjectId = companyId,
                Subject = "Welcome to the Company"
            };

            _generateEmailWrapper.GenerateAndSendEmail(generateEmail, user);
        }*/

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
            var serverFileName = "userId-" + company.Id.ToString() + "-" + guidStr;

            model.ServerFileName = serverFileName;
            var filePath = await _uploadService.Upload(model);
            company.OriginalFileName = model.File.FileName;
            company.FotoUrl = filePath.Url;
            company.ServerFileName = filePath.ServerFileName + model.File.FileName;

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

            await _uploadService.Delete(company.ServerFileName, "userfotoscontainer");
            company.ServerFileName = null;
            company.FotoUrl = null;
            company.OriginalFileName = null;
            _contextManager.Save();
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
                    UserRole = 3
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
                        companyUser.UserRole = 2;
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
            if (userError == "Something wrong with this email(s):")
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
    }
} 
