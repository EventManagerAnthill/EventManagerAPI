using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class EventCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public EventTypes Type { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime HoldingDate { get; set; } = DateTime.UtcNow.Date;

        [Required]
        public int CompanyId { get; set; }
    }
}
