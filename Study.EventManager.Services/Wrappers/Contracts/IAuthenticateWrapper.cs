using Study.EventManager.Services.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Wrappers.Contracts
{
    public interface IAuthenticateWrapper
    {
        Task<FacebookTokenAPIModel> GetFacebookToken(string idToken);
        Task<FacebookUserAPIModel> GetFacebookUserData(string fbUserId, string idToken);
    }
}
