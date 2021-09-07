using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class UserUpdatePasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
