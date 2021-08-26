using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Models.ServiceModel
{
    public class FileSendEmail
    {
        public IFormFile PdfFile { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] FileBytes { get; set; }

    }
}
