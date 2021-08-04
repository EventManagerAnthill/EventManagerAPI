using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class GenerateEmailDto
    {
        public string UrlAdress { get; set; }

        public string EmailMainText { get; set; }

        public int ObjectId { get; set; }
    }
}
