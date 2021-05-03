using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Model
{
    public class Event
    {   
        public int Id { get; set; } 

        public string Name { get; set; } 

        public DateTime CreateDate { get; set; } 

        public DateTime HoldingDate { get; set; } 

        public EventTypes Type { get; set; } 

        public int UserId { get; set; } 

        public virtual User User { get; set; }

        public string Description { get; set; }
    }
}
