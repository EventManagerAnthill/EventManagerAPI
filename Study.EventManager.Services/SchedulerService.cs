using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services
{
    internal class SchedulerService: ISchedulerService
    {
        private readonly IMapper _mapper;
        private IContextManager _contextManager;
        private IEmailWrapper _emailWrapper;
        private IGenerateEmailWrapper _generateEmailWrapper;

        public SchedulerService(IMapper mapper, IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IEmailWrapper emailWrapper)
        {
            _mapper = mapper;
            _contextManager = contextManager;
            _emailWrapper = emailWrapper;
            _generateEmailWrapper = generateEmailWrapper;
        }

        public void Dispose()
        {
            // public interface ISchedulerService: IDisposable ...
            // Need for Create this Scoped Service via IServiceScopeFactory from Singleton (IScheduledTask)!
        }

        public async Task ToDoSomething()
        {
            
        }

        public async Task CheckFinishedEvents()
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var listEvents = repo.GetAll();
            foreach (var oneEvent in listEvents)
            {
                if (oneEvent.HoldingDate.Date == DateTime.UtcNow.Date)
                {
                    var repoEventUsers = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
                    var eventListUsers = repoEventUsers.GetAll().Where(x => x.EventId == oneEvent.Id).Select(x => x.User);

                    foreach (var user in eventListUsers)
                    {
                        var generateEmail = new GenerateEmailDto
                        {
                            UrlAdress = "as" + "/signin?",
                            EmailMainText = "To leave a review follow the link",
                            ObjectId = 0,
                            Subject = "Event review"
                        };

                        var emailModel = _generateEmailWrapper.GenerateEmail(generateEmail, user);
                        _emailWrapper.SendEmail(emailModel);
                    }                    
                }
            }
        }
    }
}
