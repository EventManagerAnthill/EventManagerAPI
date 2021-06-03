using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class EmailDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public string ToAddress { get; set; }

        public string ToName { get; set; }
    }
}
