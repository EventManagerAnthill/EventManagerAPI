using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Model
{
    public class SubscriptionRates
    {
        internal SubscriptionRates()
        { }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ValidityDays{ get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public bool isFree { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
