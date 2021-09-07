using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Model
{
    public class Event
    {
        internal Event()
        { }

        public Event(string eventName, int eventUserId, EventTypes eventType, string companyDescription)
        {
            if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventUserId.ToString()) || string.IsNullOrEmpty(eventType.ToString()) || string.IsNullOrEmpty(companyDescription))
                throw new ArgumentNullException("Fill all required fields");

            Name = eventName;
            UserId = eventUserId;
            Type = eventType;
            Description = companyDescription;
            Del = 0;
        }
        public int Id { get; set; } 

        public string Name { get; set; } 

        public DateTime CreateDate { get; set; } 

        public DateTime HoldingDate { get; set; } 

        public EventTypes Type { get; set; } 

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int Status { get; set; } = 0;

        public string Description { get; set; }

        public int Del { get; set; } = 0;

        public string OriginalFileName { get; set; }

        public string ServerFileName { get; set; }

        public string FotoUrl { get; set; }       
    }
}
