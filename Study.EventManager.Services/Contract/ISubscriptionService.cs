using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface ISubscriptionService
    {
        void CheckSubscription(int companyId);

        IEnumerable<SubscriptionRatesDto> GetAllSubscriptions();

        SubscriptionRatesDto AddNewSubscription(SubscriptionCreateDto dto);

        Task<SubscriptionRatesDto> UploadSubFoto(int subId, FileDto model);

        SubscriptionRatesDto UpdateSubscription(int id, SubscriptionRatesDto dto);

        void DeleteSubscription(int id);

        Task<SubscriptionRatesDto> DeleteSubscriptionFoto(int subId);

        string PromoteSubscription(int CompanyId, int SubscriptionId);
    }
}
