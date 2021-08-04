using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Exceptions
{
    class EventExceptions : Exception
    {
        public EventExceptions(string message)
            : base(message)
        {

        }
    }
}
