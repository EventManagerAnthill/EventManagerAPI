using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Study.EventManager.Model.Enums
{
    public enum CompanyUserRoleEnum : int
    {
        [Description("Company user")]
        User = 3,
        [Description("Company admin")]
        Admin = 2,
        [Description("Company owner")]
        Owner = 1,
    }
}
