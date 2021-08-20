using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Models
{
    public class FileAPIModel
    {
        public IFormFile ImageFile { get; set; }
    }
}
