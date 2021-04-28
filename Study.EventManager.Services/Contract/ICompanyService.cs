using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface ICompanyService
    {
        CompanyDto GetCompany(int id);
        CompanyDto CreateCompany(CompanyDto dto);
        CompanyDto UpdateCompany(int id, CompanyDto dto);
        CompanyDto DeleteCompany(int id);
    }
}
