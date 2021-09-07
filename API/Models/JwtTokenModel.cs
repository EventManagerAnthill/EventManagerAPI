using Study.EventManager.Model.Enums;
using System;

namespace API.Models
{
    public class JwtTokenModel
    {
        public string access_token { get; set; }
        public string email { get; set; }
    }
}
