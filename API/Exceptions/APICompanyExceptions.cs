using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Exceptions
{
    public class APICompanyExceptions : Exception
    {
        public APICompanyExceptions(string message)
            : base(message)
        {

        }
    }
}
