using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{

    public class SocialNetworkIdTokenModel
    {  
        [Required]
        public string IdToken { get; set; }

        public string Url { get; set; }
    }
}
