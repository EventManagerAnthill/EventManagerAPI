using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class EventCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public EventTypes Type { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime HoldingDate { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
