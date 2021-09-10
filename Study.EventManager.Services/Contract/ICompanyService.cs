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
        CompanyDto GetCompany(int id, int userId);
        CompanyDto CreateCompany(CompanyCreateDto dto);
        CompanyDto UpdateCompany(int id, CompanyDto dto);        
        void DeleteCompany(int id);
        CompanyDto MakeCompanyDel(int id);   
        string AcceptInvitation(int companyId, string Email);     
        Task<CompanyDto> UploadCompanyFoto(int id, FileDto model);
        string GenerateLinkToJoin(int CompanyId, DateTime date);
        string JoinCompanyViaLink(int CompanyId, string email, string Code);
        void InviteUsersToCompany(CompanyTreatmentUsersModel model);
        string AppointUserAsAdmin(CompanyTreatmentUsersModel model);
        void AddUsersCSV(int CompanyId, IFormFile file);
        Task<CompanyDto> DeleteCompanyFoto(int CompanyId);        
        void DeleteCompanyMember(int companyId, int userId);
        void DemoteAdminToUser(int companyId, int userId);

        PagedCompaniesDto GetAllByOwner(int userId, int page, int pageSize, string companyName);
        PagedCompaniesDto GetAllByUser(int userId, int page, int pageSize, string companyName);
        PagedEventsDto GetCompanyEvents(int CompanyId, int page, int pageSize);
        PagedUsersDto GetCompanyUsers(int CompanyId, int page, int pageSize, string firstName, string lastName);
    }
}
