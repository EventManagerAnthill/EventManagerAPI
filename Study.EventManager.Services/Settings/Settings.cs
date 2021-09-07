﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services
{
    public class Settings
    {
        public string ConnectionString { get; set; }

        public EmailSettings MailSettings { get; set; }

        public string FrontUrl { get; set; }

        public string MailKey { get; set; }
    }
}
