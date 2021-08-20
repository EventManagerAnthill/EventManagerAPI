using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class FileDto
    {
        public IFormFile ImageFile { get; set; }

        public string Email { get; set; }

        public string ServerFileName { get; set; }

        public string OriginalFileName { get; set; }
        
        public string Url { get; set; }

        public string Container { get; set; }
    }
}
