using Study.EventManager.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Wrappers.Contracts
{
    internal interface IEmailWrapper
    {
        void SendEmail(EmailDto model);
    }
}
