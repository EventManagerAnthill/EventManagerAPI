using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Exceptions
{
    public class APIValidationException : Exception
    {
        public APIValidationException(string message)
            : base(message)
        {

        }
    }
}
