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
        private readonly string _urlAdress;

        public SchedulerService(IMapper mapper, IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IEmailWrapper emailWrapper, Settings settings)
        {
            _mapper = mapper;
            _contextManager = contextManager;
            _emailWrapper = emailWrapper;
            _generateEmailWrapper = generateEmailWrapper;
            _urlAdress = settings.FrontUrl;
        }

        public void Dispose()
        {
            // public interface ISchedulerService: IDisposable ...
            // Need for Create this Scoped Service via IServiceScopeFactory from Singleton (IScheduledTask)!
        }

        public async Task SubscriptionEmail()
        {
            var repo = _contextManager.CreateRepositiry<ICompanySubRepo>();
            var listCompanySub = repo.GetListOfExpiringSubs();

            foreach (var oneCompSub in listCompanySub)
            {                
                var generateEmail = new GenerateEmailDto
                {
                    UrlAdress = _urlAdress + "/company/" + oneCompSub.CompanyId,
                    EmailMainText = "Your subscription to " + oneCompSub.Company.Name + " will expire in 5 days",
                    ObjectId = 0,
                    Subject = "Sudscription"
                };

                var emailModel = _generateEmailWrapper.GenerateEmail(generateEmail, oneCompSub.User);
                _emailWrapper.SendEmail(emailModel);
            }
        }

        public async Task CheckFinishedEvents()
        {
            var repo = _contextManager.CreateRepositiry<IEventRepo>();
            var listEvents = repo.GetAll(x => x.HoldingDate.Date == DateTime.UtcNow.Date);

            foreach (var oneEvent in listEvents)
            {
                var repoEventUsers = _contextManager.CreateRepositiry<IEventUserLinkRepo>();
                var eventListUsers = repoEventUsers.GetListUsers(oneEvent.Id);

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
