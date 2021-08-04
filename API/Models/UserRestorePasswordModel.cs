using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserRestorePasswordModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string validTo { get; set; }

        [Required]
        public string code { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
