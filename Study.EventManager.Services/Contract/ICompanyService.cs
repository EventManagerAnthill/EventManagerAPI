using Study.EventManager.Services.Dto;
using System.Collections.Generic;

namespace Study.EventManager.Services.Contract
{
    public interface ICompanyService
    {
        CompanyDto GetCompany(int id);
        CompanyDto CreateCompany(CompanyCreateDto dto);
        CompanyDto UpdateCompany(int id, CompanyDto dto);
        IEnumerable<CompanyDto> GetAllByOwner(string email = null);
        IEnumerable<CompanyDto> GetAllByUser(string email = null);
        void DeleteCompany(int id);
        CompanyDto MakeCompanyDel(int id, CompanyDto dto);
        public void sendInviteEmail(int companyId, string Email);        
        public string AcceptInvitation(int companyId, string Email);
    }
}
