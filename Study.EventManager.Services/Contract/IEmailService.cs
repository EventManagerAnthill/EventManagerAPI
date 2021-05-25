using MailService.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.WebApi.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
    }
}
