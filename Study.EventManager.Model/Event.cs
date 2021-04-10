using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Model
{
    public class Event
    {   
        public int Id { get; set; } 

        public string Name { get; set; } //name of event

        public DateTime Create_dt { get; set; } 

        public DateTime Holding_dt { get; set; } //holding date

        public int Type_id { get; set; } //1 - public; 2 - privite; 

        public int User_id { get; set; } //user who is responsible for event 

        public string Description { get; set; } // event description

    }
}
