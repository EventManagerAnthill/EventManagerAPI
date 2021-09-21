using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Study.EventManager.Model.Enums
{
    public enum ObjectDel : int
    {
        [Description("If status company/event is active")]
        Active = 0,
        [Description("If status company/event is delete")]
        Del = 1,
    }
}
