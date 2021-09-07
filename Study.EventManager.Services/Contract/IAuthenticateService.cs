using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Contract
{
    public interface IAuthenticateService
    {
        UserDto Authenticate(string email, string password);
        string VerifyUrlEmail(string email, string code);
        void sendRestoreEmail(string email);
        string restorePass(string email, string password, string code);
        bool SocialNetworksAuthenticate(string email, string name, string givenName, string familyName);
    }
}
