using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class JoinCompanyModel
    {
        public string Email { get; set; }

        public int CompanyId { get; set; }

        public string Code { get; set; }
    }
}
