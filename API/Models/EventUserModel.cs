using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class EventUserModel
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
