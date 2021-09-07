using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Models.ServiceModel;
using Study.EventManager.Services.Wrappers.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Wrappers
{
    internal class EmailWrapper //: IEmailWrapper
    {
        private readonly EmailSettings _mailSettings;
        
        public EmailWrapper(Settings settings)
        {
            _mailSettings = settings.MailSettings;
        }

        public void SendEmail(EmailDto model, FileSendEmail file = null)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            email.To.Add(new MailboxAddress(model.ToName, model.ToAddress));

            
            email.Subject = model.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = model.Body;

            if (!(file == null)) 
            {
                builder.Attachments.Add(file.FileName, file.FileBytes); 
            }
       
            email.Body = builder.ToMessageBody();           
            using var smtp = new SmtpClient();
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
