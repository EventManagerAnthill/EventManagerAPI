using AutoMapper;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Mappers
{
    public class CompanyMapper : Profile
    {
        public CompanyMapper()
        {
            CreateMap<CompanyDto, Company>().ReverseMap();

            CreateMap<CompanyCreateDto, Company>().ReverseMap();
        }
    }
}
