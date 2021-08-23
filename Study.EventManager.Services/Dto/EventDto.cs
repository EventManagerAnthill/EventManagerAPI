using Study.EventManager.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class EventDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime HoldingDate { get; set; }

        public EventTypes Type { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int Del { get; set; }

        public string OriginalFileName { get; set; }

        public string ServerFileName { get; set; }

        public string FotoUrl { get; set; }
    }
}
