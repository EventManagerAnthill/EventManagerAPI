using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Study.EventManager.Data.Contract;
using Study.EventManager.Data.Repositiry;
using Study.EventManager.Model;
using Study.EventManager.Model.Enums;
using Study.EventManager.Services.Contract;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services
{
    internal class SubscriptionService : ISubscriptionService
    {
        private readonly IMapper _mapper;
        private IContextManager _contextManager;
        private IUploadService _uploadService;

        public SubscriptionService(IMapper mapper, IContextManager contextManager, IGenerateEmailWrapper generateEmailWrapper, IEmailWrapper emailWrapper, IUploadService uploadService)
        {
            _mapper = mapper;
            _contextManager = contextManager;            
            _uploadService = uploadService;
        }

        public void CheckSubscription(int companyId)
        {
            try
            {
                var repoSub = _contextManager.CreateRepositiry<ICompanySubRepo>();
                var sub = repoSub.GetStatusOfSubscription(companyId);

                if (!sub)
                {
                    throw new ValidationException("Company subscription is finished.");
                }
            }
            catch
            {
                throw new ValidationException("Company subscription is finished.");
            }
        }

        public IEnumerable<SubscriptionRatesDto> GetAllSubscriptions()
        {
            var repo = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var data = repo.GetAll();
            return data.Select(x => _mapper.Map<SubscriptionRatesDto>(x)).ToList();
        }

        public SubscriptionRatesDto AddNewSubscription(SubscriptionCreateDto dto)
        {
            var reposubscription = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var sub = _mapper.Map<SubscriptionRates>(dto);           
            reposubscription.Add(sub);
            _contextManager.Save();

            return _mapper.Map<SubscriptionRatesDto>(sub);
        }

        public async Task<SubscriptionRatesDto> UploadSubFoto(int subId, FileDto model)
        {           
            var repo = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var sub = repo.GetById(subId);

            if (sub == null)
            {
                throw new ValidationException("Subscription not found");
            }

            if (!(sub.ServerFileName == null))
            {
                await _uploadService.Delete(sub.ServerFileName, model.Container);
            }

            var guidStr = Guid.NewGuid().ToString();
            var serverFileName = "subId-" + sub.Id.ToString() + "-" + guidStr;

            model.ServerFileName = serverFileName;
            var filePath = await _uploadService.Upload(model);
            sub.OriginalFileName = model.File.FileName;
            sub.FotoUrl = filePath.Url;
            sub.ServerFileName = filePath.ServerFileName + model.File.FileName;

            _contextManager.Save();

            return _mapper.Map<SubscriptionRatesDto>(sub);
        }

        public SubscriptionRatesDto UpdateSubscription(int id, SubscriptionRatesDto dto)
        {
            var repo = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var sub = repo.GetById(id);

            if (sub == null)
            {
                throw new ValidationException("Subscription not found");
            }

            sub.Name = dto.Name;
            sub.isFree = dto.isFree;
            sub.Price = dto.Price;
            sub.ValidityDays = dto.ValidityDays;
            sub.Description = dto.Description;           

            _contextManager.Save();

            return _mapper.Map<SubscriptionRatesDto>(sub);
        }

        public void DeleteSubscription(int subId)
        {
            var repo = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var sub = repo.GetById(subId);
            sub.Del = (int)ObjectDel.Del;
            _contextManager.Save();  
        }

        public async Task<SubscriptionRatesDto> DeleteSubscriptionFoto(int subId)
        {
            var repo = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var sub = repo.GetById(subId);

            if (sub == null)
            {
                throw new ValidationException("Subscription not found");
            }

            await _uploadService.Delete(sub.ServerFileName, "subscriptionfotoscontainer");
            sub.ServerFileName = null;
            sub.FotoUrl = null;
            sub.OriginalFileName = null;
            _contextManager.Save();
            var subDto = _mapper.Map<SubscriptionRatesDto>(sub);
            return subDto;
        }

        public string PromoteSubscription(int CompanyId, int SubscriptionId)
        {
            var repo = _contextManager.CreateRepositiry<ISubscriptionRatesRepo>();
            var sub = repo.GetById(SubscriptionId);

            if (sub.Del == (int)ObjectDel.Del)
            {
                throw new ValidationException("Subscription is delete");
            }

            var repoCompany = _contextManager.CreateRepositiry<ICompanyRepo>();
            var company = repoCompany.GetById(CompanyId);
            
            if (company == null)
            {
                throw new ValidationException("Company not found");
            }

            var companySubRepo = _contextManager.CreateRepositiry<ICompanySubRepo>();
            var companySub = companySubRepo.GetById(CompanyId);

            if (companySub == null)
            {
                throw new ValidationException("Error.");
            }

            companySub.SubEndDt = companySub.SubEndDt.AddDays(sub.ValidityDays);
            companySub.UseTrialVersion = (int)CompanyTrialVersionEnum.AlreadyUsedTrial;
            _contextManager.Save();

            return "subscription renewed successfully";
        }
    }
}
