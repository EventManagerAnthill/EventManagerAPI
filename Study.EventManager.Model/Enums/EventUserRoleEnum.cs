using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Study.EventManager.Model.Enums
{
    public enum EventUserRoleEnum : int
    {
        [Description("Event user")]
        User = 3,
        [Description("Event admin")]
        Admin = 2,
        [Description("Event owner")]
        Owner = 1,
    }
}
