using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Model
{
    public class CompanySubscription
    {
        internal CompanySubscription()
        { }

        public int Id { get; set; }

        [Required]
        public int SubscriptionId { get; set; }
        public virtual SubscriptionRates Subscription { get; set; }

        [Required]
        public int CompanyId { get; set; }      
        public virtual Company Company { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public DateTime SubEndDt { get; set; } = DateTime.UtcNow.Date;

        [Required]
        public int UseTrialVersion { get; set; } = (int)CompanyTrialVersionEnum.AlreadyUsedTrial;
    }
}
