using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Wrappers.Contracts
{
    internal interface IGenerateEmailWrapper
    {
        public EmailDto GenerateEmail(GenerateEmailDto dto, User user);

        public string GetUrl(string email, string urlAdress);

        public string GetHashString(string s);

        public void GenerateAndSendEmail(GenerateEmailDto dto, User user);
    }
}
