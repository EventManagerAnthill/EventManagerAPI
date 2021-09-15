using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class SubscriptionRatesDto
    {
        public int Id { get; set; }

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

        public int Del { get; set; }
    }
}
