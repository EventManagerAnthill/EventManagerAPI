using Microsoft.AspNetCore.Http;
using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Contract
{
    public interface ICompanyService
    {
        CompanyDto GetCompany(int id);
        CompanyDto CreateCompany(CompanyCreateDto dto);
        CompanyDto UpdateCompany(int id, CompanyDto dto);
        IEnumerable<CompanyDto> GetAllByOwner(string email = null);
        List<Company> GetAllByUser(string email);
        void DeleteCompany(int id);
        CompanyDto MakeCompanyDel(int id, CompanyDto dto);
      //  public void sendInviteEmail(int companyId, string Email);        
        public string AcceptInvitation(int companyId, string Email);
        public int CountCompanyUser(int companyId);
        Task UploadCompanyFoto(int id, FileDto model);
        public string GenerateLinkToJoin(int CompanyId, DateTime date);
        public string JoinCompanyViaLink(int CompanyId, string email, string Code);
        public void InviteUsersToCompany(CompanyTreatmentUsersModel model);
        public string AppointUserAsAdmin(CompanyTreatmentUsersModel model);
        public void AddUsersCSV(int CompanyId, IFormFile file);
    }
}
