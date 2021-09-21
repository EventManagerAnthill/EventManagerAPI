using AutoMapper;
using CsvHelper;
using IronPdf;
using iText.Html2pdf;
using Microsoft.AspNetCore.Http;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Models.APIModels;
using Study.EventManager.Services.Models.ServiceModel;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    internal class EventService : IEventService
    {
        private IGenerateEmailWrapper _generateEmailWrapper;
        private IContextManager _contextManager;
        private IGenerateQRCode _generateQRCode;
        private readonly string _urlAdress;
        private IEmailWrapper _emailWrapper;
        private IUploadService _uploadService;
        private readonly IMapper _mapper;        
        private ISubscriptionService _subscriptionService;

        const string secretKey = "JoinEventViaLinkHash";

        public EventService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, Settings settings, IGenerateQRCode generateQRCode
            , IEmailWrapper emailWrapper, IUploadService uploadService, IMapper mapper, ISubscriptionService subscriptionServicer)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
            _urlAdress = settings.FrontUrl;
            _generateQRCode = generateQRCode;
            _emailWrapper = emailWrapper;
            _uploadService = uploadService;
            _mapper = mapper;
            _subscriptionService = subscriptionServicer;
        }
       
        public string AcceptInvitation(int EventId, string Email)
        {
            var repoEvent = _contextManager.CreateRepositiry<IEventRepo>();
            var companyEvent = repoEvent.GetById(EventId);

            _subscriptionService.CheckSubscription(companyEvent.CompanyId);

            if (companyEvent == null)
            {
                throw new ValidationException("Event not found.");
            }           

            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);

            if (user == null)
            {
                throw new ValidationException("User not found.");
            }
           
            var repoEventUser = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
            var eventUser = repoEventUser.GetRecordByEventAndUser(user.Id, EventId);

            if (!(eventUser == null))
            {
                throw new ValidationException("User is already added to the event.");
            }

            var entity = new EventUserLink
            {
                EventId = EventId,
                UserId = user.Id,
                UserEventRole = (int)Model.Enums.EventUserRoleEnum.User
            };
            repoEventUser.Add(entity);
      
            _contextManager.Save();

            SendEventTicket(companyEvent, user);

            return "You successfully join the Event, the ticket send to your email";
        }

        public EventDto GetEvent(int eventId, int userId)
        {
            try
            {
                var repo = _contextManager.CreateRepositiry<IEventRepo>();
                var evnt = repo.GetById(eventId);
                var result = _mapper.Map<EventDto>(evnt);

                var repoUserEvents = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
                result.UserRole = repoUserEvents.GetUserRole(userId, eventId);

                if (result.Type == Model.Enums.EventTypes.Private && result.UserRole == 0)
                {
                    throw new ValidationException("Can't show the event");
                }

                return result;
            }
            catch
            {
                throw new ValidationException("Can't show event");
            }
        }

        public EventDto CreateEvent(EventCreateDto dto)
        {
            _subscriptionService.CheckSubscription(dto.CompanyId);

            try
            {
                var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
                var user = repoUser.GetById(dto.UserId);

                if (user == null)
                {
                    throw new ValidationException("User not found");
                }  
                var entity = _mapper.Map<Event>(dto);
                var repo = _contextManager.CreateRepositiry<IEventRepo>();
                repo.Add(entity);
                _contextManager.Save();

                //add user to the event
                var repoEventUser = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
                var eventUser = new EventUserLink
                {
                    EventId = entity.Id,
                    UserId = user.Id,
                    UserEventRole = (int)Model.Enums.EventUserRoleEnum.Owner
                };
                repoEventUser.Add(eventUser);
                _contextManager.Save();

                return _mapper.Map<EventDto>(entity);
            }
            catch
            {
                throw new ValidationException("Sorry, unexpected error.");
            }
        }

        public EventDto UpdateEvent(int id, EventDto dto)
        {
            _subscriptionService.CheckSubscription(dto.CompanyId);
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);    
            
            data.Name = dto.Name;
            data.Type = dto.Type;
            data.CreateDate = dto.CreateDate;
            data.BeginHoldingDate = dto.BeginHoldingDate;
            data.EventTimeZone = dto.EventTimeZone;
            data.HoldingDate = dto.HoldingDate;
            data.Description = dto.Description;

            _contextManager.Save();
            return _mapper.Map<EventDto>(data);
        }

        public void DeleteEvent(int id)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);
            repo.Delete(data);
            _contextManager.Save();
        }

        public IEnumerable<EventDto> GetAll()
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetAll();
            return data.Select(x => _mapper.Map<EventDto>(x)).ToList();
        }

        public EventDto GetEventByUserOwnerId(int UserId)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetAllEventsByUser(UserId);
            var result = _mapper.Map<EventDto>(data);
            return result;
        }     
     
        public EventDto MakeEventDel(int EventId, EventDto dto)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(EventId);
            data.Del = dto.Del;
            _contextManager.Save();

            SendEmailToUsers(EventId, "cancellation of an event", "The Event " + "'" + data.Name + "' was canceled");
            return _mapper.Map<EventDto>(data); 
        }
        
        public EventDto CancelEvent(int EventId, EventDto dto)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(EventId);
            data.Status = dto.Status;
            _contextManager.Save();

            SendEmailToUsers(EventId, "cancellation of an event", "The Event " + "'" + data.Name + "' was canceled");

            return _mapper.Map<EventDto>(data);
        }
        
        public void SendEmailToUsers(int EventId, string subject, string mainMassage)
        {
            var eventRepo = _contextManager.CreateRepositiry<IEventRepo>();
            var getEvent = eventRepo.GetById(EventId);

            _subscriptionService.CheckSubscription(getEvent.CompanyId);

            if (getEvent == null)
            {
                throw new ValidationException("Event not found");
            }

            var repo = _contextManager.CreateRepositiry<IEventUserLinkRepo>();                        
            var data = repo.GetAll(x => x.EventId == EventId);
            var listUsers = repo.GetAllUsers(EventId);

            if (listUsers == null)
            {
                throw new ValidationException("Users not found in this event");
            }

            var generateEmail = new GenerateEmailDto
            {                
                UrlAdress = _urlAdress+ "/event/" + EventId + "?",                
                EmailMainText = mainMassage,
                ObjectId = EventId,
                Subject = subject
            };

            listUsers.Select(x => x.User);

            foreach (EventUserLink user in listUsers)
            {
                Thread thread = new Thread(() => _emailWrapper.SendEmail(_generateEmailWrapper.GenerateEmail(generateEmail, user.User)));
                thread.Start(); 
            }
        }

        public void SendEventTicket(Event eventU, User user)
        {
            _subscriptionService.CheckSubscription(eventU.CompanyId);

            var pdfBytes = GetEventTicket(eventU, user);
          
            var file = new FileSendEmail
            {             
                FileBytes = pdfBytes,                
                FileName = eventU.Name + "_Ticket.pdf"
            };
           
            var generateEmail = new GenerateEmailDto
            {
                UrlAdress = _urlAdress + "/event/" + eventU.Id + "?",
                EmailMainText = "Your Ticket on Event",
                ObjectId = eventU.Id,
                Subject = "event ticket"
            };

            var emailModel = _generateEmailWrapper.GenerateEmail(generateEmail, user);

            _emailWrapper.SendEmail(emailModel, file);
        }

        public byte[] GetEventTicket(Event eventU, User user)
        {
            //string FileInputPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\Study.EventManager.Services", "Resources", "TikectTemplateIn.html");                        
            string FileInputPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "TikectTemplateIn.html");                        
            StreamReader str = new StreamReader(FileInputPath);
            string MailText = str.ReadToEnd();
            str.Close();

            string foroUrl;
            if (eventU.FotoUrl == null)
            {
                foroUrl = "src='https://eventmanagerstoragefiles.blob.core.windows.net/eventfotoscontainer/logoEvent.JPG'";
            }
            else
            {
                foroUrl = "src='" + eventU.FotoUrl+"'";
            }
            var imgBase64 = _generateQRCode.QRCode(_urlAdress);
            var mailText = MailText.Replace("[EventFoto]", foroUrl)
                .Replace("[USERNAME]", user.FirstName + " " + user.LastName)
                .Replace("[NAME]", eventU.Name)
                .Replace("[HOLDING]", eventU.HoldingDate.ToString())
                .Replace("[DESCRIPTION]", eventU.Description)
                .Replace("[QRCode]", imgBase64);


            MemoryStream memStream = new MemoryStream();           
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(mailText, memStream);
            byte[] pdfByte = memStream.ToArray();
   
            return pdfByte;               
        }

        public PagedEventsDto GetAllByUser(int userId, int page, int pageSize, string eventName)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetById(userId);

            var repoUserEvents = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
            var listUserEvents = repoUserEvents.GetEventsByUser(user.Id, page, pageSize, eventName);

            var eventListDto = _mapper.Map<List<EventDto>>(listUserEvents);
            var eventIdList = eventListDto.Select(c => c.Id).ToList();
            var eventLinks = repoUserEvents.GetCompanyUserLinkListForUser(userId, eventIdList);

            foreach (var oneEvent in eventListDto)
            {
                var thisEventLink = eventLinks.Where(c => c.EventId == oneEvent.Id).FirstOrDefault();
                if (thisEventLink != null)
                {
                    oneEvent.UserRole = thisEventLink.UserEventRole;
                }
            }

            var retDto = new PagedEventsDto()
            {
                Events = eventListDto,
                Paging = new PagingDto()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = repoUserEvents.GetEventsByUserCount(user.Id)
                }
            };

            return retDto;
        }

        public EventReviewDto EventReview(EventReviewCreateDto dto)
        {
            var repoReview = _contextManager.CreateRepositiry<IEventReviewRepo>();
            var review = repoReview.GetAll(x => x.UserId == dto.UserId && x.EventId == dto.EventId);
            if (review != null)
            {
                throw new ValidationException("You have already leave feedback on this event");
            }
            var entity = _mapper.Map<EventReview>(dto);

            repoReview.Add(entity);
            _contextManager.Save();

            var eventDto = _mapper.Map<EventReviewDto>(entity);
            return eventDto;
        }

        public async Task<EventDto> UploadEventFoto(int EventId, FileDto model)
        {     
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var eventF = repo.GetById(EventId);

            _subscriptionService.CheckSubscription(eventF.CompanyId);

            if (eventF == null)
            {
                throw new ValidationException("Event not found");
            }

            if (!(eventF.ServerFileName == null))
            {
                await _uploadService.Delete(eventF.ServerFileName, model.Container);
            }

            var guidStr = Guid.NewGuid().ToString();
            var serverFileName = "userId-" + eventF.Id.ToString() + "-" + guidStr;

            model.ServerFileName = serverFileName;
            var filePath = await _uploadService.Upload(model);
            eventF.OriginalFileName = model.File.FileName;
            eventF.FotoUrl = filePath.Url;
            eventF.ServerFileName = filePath.ServerFileName + model.File.FileName;

            _contextManager.Save();

            return _mapper.Map<EventDto>(eventF);
        }

        public async Task<EventDto> DeleteEventFoto(int EventId)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var eventFile = repo.GetById(EventId);

            if (eventFile == null)
            {
                throw new Exceptions.ValidationException("Company not found");
            }

            await _uploadService.Delete(eventFile.ServerFileName, "eventfotoscontainer");
            eventFile.ServerFileName = null;
            eventFile.FotoUrl = null;
            eventFile.OriginalFileName = null;
            _contextManager.Save();
            var eventyDto = _mapper.Map<EventDto>(eventFile);
            return eventyDto;
        }

        public void AddUsersCSV(int EventId, IFormFile file)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var eventRecord = repo.GetById(EventId);
            _subscriptionService.CheckSubscription(eventRecord.CompanyId);

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

            var generateEmail = new GenerateEmailDto
            {
                UrlAdress = _urlAdress + "/event/" + EventId + "?",
                EmailMainText = "Invitation to the event, for confirmation follow the link",
                ObjectId = EventId,
                Subject = "Welcome to the Event"
            };

            foreach (string email in listEmails)
            {
                EmailFunctionality(email, EventId, generateEmail);
            }
        }

        public void EmailFunctionality(string email, int EventId, GenerateEmailDto dto)
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
                var repo = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
                var companyUser = repo.GetRecordByEventAndUser(user.Id, EventId);

                if (companyUser == null)
                {
                    Thread thread = new Thread(() => _generateEmailWrapper.GenerateAndSendEmail(dto, user));
                    thread.Start();
                }
            }
        }

        public void InviteUsersToEvent(EventTreatmentUsersModel model)
        {            
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var eventRecord = repo.GetById(model.EventId);

            _subscriptionService.CheckSubscription(eventRecord.CompanyId);

            if (eventRecord == null)
            {
                throw new ValidationException("Event not found.");
            }

            var generateEmail = new GenerateEmailDto
            {
                UrlAdress = _urlAdress + "/event/" + model.EventId + "?",
                EmailMainText = "Invitation to the event, for confirmation follow the link",
                ObjectId = model.EventId,
                Subject = "Welcome to the Event"
            };

            foreach (string email in model.Email)
            {
                EmailFunctionality(email, model.EventId, generateEmail);
            }
        }

        public string GenerateLinkToJoin(int EventId, DateTime date)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var eventRecord = repo.GetById(EventId);

            _subscriptionService.CheckSubscription(eventRecord.CompanyId);

            if (eventRecord == null)
            {
                throw new ValidationException("Company not found");
            }

            string hashUrl = GetHashString(secretKey + eventRecord.Name);

            hashUrl = System.Web.HttpUtility.UrlEncode(hashUrl);

            string url;
            if (!(date == DateTime.MinValue))
            {
                var dateStr = date.ToString("dd.MM.yyyy");
                dateStr = System.Web.HttpUtility.UrlEncode(dateStr);
                url = "?" + "eventid=" + eventRecord.Id + "&validTo=" + dateStr + "&code={" + hashUrl + "}";
            }
            else
            {
                url = "?" + "eventid=" + eventRecord.Id + "&code={" + hashUrl + "}";
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

        public string JoinEventViaLink(int EventId, string email, string Code)
        {            
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var eventRecord = repo.GetById(EventId);

            _subscriptionService.CheckSubscription(eventRecord.CompanyId);

            if (eventRecord == null)
            {
                throw new ValidationException("Event not found");
            }

            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(email);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            string hashUrl = "{" + GetHashString(secretKey + eventRecord.Name) + "}";

            if (Code == hashUrl)
            {
                var repoCompUser = _contextManager.CreateRepositiry<ICompanyUserLinkRepo>();
                var companyUser = repoCompUser.GetRecordByCompanyAndUser(user.Id, EventId);

                if (!(companyUser == null))
                {
                    throw new ValidationException("User is already added to the event.");
                }

                var entity = new CompanyUserLink
                {
                    CompanyId = EventId,
                    UserId = user.Id,
                    UserCompanyRole = (int)Model.Enums.EventUserRoleEnum.User
                };
                repoCompUser.Add(entity);
                _contextManager.Save();

                return "You are join the event";
            }
            else
            {
                return "Sorry, unexpected error";
            }
        }
    }
}

