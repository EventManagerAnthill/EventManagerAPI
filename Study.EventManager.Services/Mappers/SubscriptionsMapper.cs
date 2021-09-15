using AutoMapper;
using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Mappers
{
    public class SubscriptionsMapper : Profile
    {
        public SubscriptionsMapper()
        {
            CreateMap<SubscriptionRatesDto, SubscriptionRates>().ReverseMap();    
            
            CreateMap<SubscriptionCreateDto, SubscriptionRates>().ReverseMap();            
        }
    }
}
