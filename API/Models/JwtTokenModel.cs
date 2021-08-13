using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class JwtTokenModel
    {
        public string access_token { get; set; }
        public string email { get; set; }
    }
}
