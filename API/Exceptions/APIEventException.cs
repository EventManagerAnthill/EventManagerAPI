using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class APIEventException : Exception
    {
        public APIEventException(string message)
            : base(message)
        {

        }
    }
}
