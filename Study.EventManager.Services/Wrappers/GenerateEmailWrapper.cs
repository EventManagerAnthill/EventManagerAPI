using Study.EventManager.Model;
using Study.EventManager.Services.Dto;
using Study.EventManager.Services.Exceptions;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Study.EventManager.Services.Wrappers
{
    internal class GenerateEmailWrapper : IGenerateEmailWrapper
    {
        private IEmailWrapper _emailWrapper;
        const string secretKey = "emailVerificationHash";

        public GenerateEmailWrapper(IEmailWrapper emailWrapper)
        {
            _emailWrapper = emailWrapper;
        }

        public void GenerateAndSendEmail(GenerateEmailDto dto, User user)
        {
            try
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\Study.EventManager.Services", "Resources", "WelcomeTemplate.html");
                //string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "WelcomeTemplate.html");

                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                var url = "";

                if (dto.ObjectId == 0) 
                {
                    url = GetUrl(user.Email, dto.UrlAdress); 
                }
                else
                {
                    url = GetObjectUrl(dto.ObjectId, user.Email, dto.UrlAdress);
                }

                var mainText = dto.EmailMainText;

                var mailText = MailText.Replace("[username]", user.FirstName + " " + user.LastName).Replace("[email]", user.Email).Replace("[verifiedLink]", url).Replace("[mainText]", mainText);

                var emailModel = new EmailDto
                {
                    Subject = $"{dto.Subject} {user.Email}",
                    Body = mailText,
                    ToAddress = user.Email,
                    ToName = user.Username
                };

                _emailWrapper.SendEmail(emailModel);
            }
            catch
            {
                 throw new GenerateEmailExceptions("Sorry, unexpected error.");
            }
        }

        public string GetUrl(string email, string urlAdress)
        {
            string hashUrl = GetHashString(secretKey + email);
            var date = DateTime.UtcNow.Date;

            date = date.AddDays(7);
            var dateStr = date.ToString("dd.MM.yyyy");

            email = System.Web.HttpUtility.UrlEncode(email);
            dateStr = System.Web.HttpUtility.UrlEncode(dateStr);
            hashUrl = System.Web.HttpUtility.UrlEncode(hashUrl);

            string url = "email=" + email + "&validTo=" + dateStr + "&code={" + hashUrl + "}";
            //url = System.Web.HttpUtility.UrlEncode(url);

            url = urlAdress + url;
            return url;
        }

        public string GetObjectUrl(int ObjectId, string email, string urlAdress)
        {
            email = System.Web.HttpUtility.UrlEncode(email);
            var url = urlAdress + "email=" + email + "&ObjectId=" + ObjectId;
            return url;
        }

        public string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
    }
}
