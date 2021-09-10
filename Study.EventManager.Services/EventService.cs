using AutoMapper;
using IronPdf;
using iText.Html2pdf;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Models.ServiceModel;
using Study.EventManager.Services.Wrappers.Contracts;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

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

        public EventService(IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, Settings settings, IGenerateQRCode generateQRCode
            , IEmailWrapper emailWrapper, IUploadService uploadService, IMapper mapper)
        {
            _generateEmailWrapper = generateEmailWrapper;
            _contextManager = contextManager;
            _urlAdress = settings.FrontUrl;
            _generateQRCode = generateQRCode;
            _emailWrapper = emailWrapper;
            _uploadService = uploadService;
            _mapper = mapper;
        }
       
        public string AcceptInvitation(int EventId, string Email)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(Email);

            if (user == null)
            {
                throw new ValidationException("User not found.");
            }

            var repoEvent = _contextManager.CreateRepositiry<IEventRepo>();
            var companyEvent = repoEvent.GetById(EventId);

            if (companyEvent == null)
            {
                throw new ValidationException("Event not found.");
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

        public EventDto GetEvent(int id)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);
            var result = _mapper.Map<EventDto>(data);
            return result;
        }

        public EventDto CreateEvent(EventCreateDto dto)
        {
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
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(id);    
            
            data.Name = dto.Name;
            data.Type = dto.Type;
            data.CreateDate = dto.CreateDate;
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

        public EventDto GetEventByUserId(int UserId)
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

            sendEmailToUsers(EventId, "cancellation of an event", "The Event " + "'" + data.Name + "' was canceled");
            return _mapper.Map<EventDto>(data); 
        }
        
        public EventDto CancelEvent(int EventId, EventDto dto)
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var data = repo.GetById(EventId);
            data.Status = dto.Status;
            _contextManager.Save();

            sendEmailToUsers(EventId, "cancellation of an event", "The Event " + "'" + data.Name + "' was canceled");

            return _mapper.Map<EventDto>(data);
        }
        
        public void sendEmailToUsers(int EventId, string subject, string mainMassage)
        {
            var eventRepo = _contextManager.CreateRepositiry<IEventRepo>();
            var getEvent = eventRepo.GetById(EventId);
            if (getEvent == null)
            {
                throw new ValidationException("Event not found");
            }

            var repo = _contextManager.CreateRepositiry<IEventUserLinkRepo>();                        
            var data = repo.GetAll().Where(x => x.EventId == EventId);
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

        public List<Event> GetAllByUser(string email)
        {
            var repoUser = _contextManager.CreateRepositiry<IUserRepo>();
            var user = repoUser.GetUserByEmail(email);

            var repoUserCompanies = _contextManager.CreateRepositiry<IEventUserLinkRepo>();

            var listUserCompanies = repoUserCompanies.GetEventsByUser(user.Id);

            return listUserCompanies;
        }

        public EventReviewDto EventReview(EventReviewCreateDto dto)
        {
            var repoReview = _contextManager.CreateRepositiry<IEventReviewRepo>();
            var review = repoReview.GetAll().Where(x => x.UserId == dto.UserId && x.EventId == dto.EventId);
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
    }
}

