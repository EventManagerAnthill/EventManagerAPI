using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Exceptions
{
    class GenerateEmailExceptions : Exception
    {
        public GenerateEmailExceptions(string message)
            : base(message)
        {

        }
    }
}
