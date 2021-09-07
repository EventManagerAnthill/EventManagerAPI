using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Study.EventManager.Model.Enums
{
    public enum UserRoleEnum : int
    {      
        [Description("site user")]
        User = 0,
        [Description("Site admin")]
        Admin = 1,
    }
}
