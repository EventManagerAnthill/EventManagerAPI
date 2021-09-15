using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Dto
{
    public class SubscriptionCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ValidityDays { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public bool isFree { get; set; }

        [Required]
        public string Description { get; set; }

        public string OriginalFileName { get; set; }

        public string ServerFileName { get; set; }

        public string FotoUrl { get; set; }
    }
}
