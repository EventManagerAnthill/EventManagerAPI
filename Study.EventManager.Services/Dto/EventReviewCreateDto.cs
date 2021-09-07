using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class EventReviewCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int StarReview { get; set; }
        public virtual Event Event { get; set; }

        public string TextReview { get; set; }
    }
}
