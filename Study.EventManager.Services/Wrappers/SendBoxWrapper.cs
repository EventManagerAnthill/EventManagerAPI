using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Models.ServiceModel;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Wrappers
{
    internal class SendBoxWrapper : IEmailWrapper
    {
        private readonly EmailSettings _mailSettings;
        private string _mailKeySettings;

        public SendBoxWrapper(Settings settings)
        {
            _mailSettings = settings.MailSettings;
            _mailKeySettings = settings.MailKey;
        }

        public string GenerateToken()
        {
            using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[8];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }

        public void SendEmail(EmailDto dto, FileSendEmail file = null)
        {
            var apiKey = _mailKeySettings;

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_mailSettings.Mail);
            var to = new EmailAddress(dto.ToAddress);
            var pwd = GenerateToken();


            //string body = string.Empty;
            //string FileTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\Study.EventManager.Services", "Resources", "WelcomeTemplate.html");
            //string FileTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "WelcomeTemplate.html");


            var htmlContent = dto.Body;
            var textContent = "";

            try
            {
                var message = MailHelper.CreateSingleEmail(from, to, dto.Subject, textContent, htmlContent);

                if (!(file == null))
                {
                    var sendFile = Convert.ToBase64String(file.FileBytes);
                    message.AddAttachment(file.FileName, sendFile);
                }

                var response = client.SendEmailAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

