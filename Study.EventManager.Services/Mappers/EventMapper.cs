using AutoMapper;
using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Mappers
{   
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<EventDto, Event>().ReverseMap();
            CreateMap<EventCreateDto, Event>().ReverseMap();

            CreateMap<EventReviewCreateDto, EventReview>();
            CreateMap<EventReviewDto, EventReview>().ReverseMap();
        }
    }
}
