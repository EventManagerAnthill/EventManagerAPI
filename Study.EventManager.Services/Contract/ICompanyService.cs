using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface ICompanyService
    {
        CompanyDto GetCompany(int id);
        CompanyDto CreateCompany(CompanyCreateDto dto);
        CompanyDto UpdateCompany(int id, CompanyDto dto);
        IEnumerable<CompanyDto> GetAll(string email = null);
        void DeleteCompany(int id);
        CompanyDto MakeCompanyDel(int id, CompanyDto dto);
        public void sendInviteEmail(int companyId, string Email);        
        public string AcceptInvitation(int companyId, string Email);
    }
}
