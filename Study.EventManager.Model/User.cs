﻿using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patron { get; set; }

        public DateTime Birth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public UserType Sex { get; set; }
    }
}
