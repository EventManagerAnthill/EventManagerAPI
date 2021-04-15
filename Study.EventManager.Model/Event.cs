using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Model
{
    public class Event
    {   
        public int Id { get; set; } 

        /// <summary>
        /// Name of Event
        /// </summary>
        public string Name { get; set; } 

        public DateTime CreateDt { get; set; } 

        public DateTime HoldingDt { get; set; } 
        /// <summary>
        /// 1 - public; 2 - private
        /// </summary>
        public int TypeId { get; set; } 
        /// <summary>
        /// User who is responsable for event
        /// </summary>
        public int UserId { get; set; } 

        public string Description { get; set; }

    }
}
