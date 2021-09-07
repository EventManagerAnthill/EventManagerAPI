using AutoMapper;
using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<UserCreateDto, User>().ReverseMap();
        }
    }
}
