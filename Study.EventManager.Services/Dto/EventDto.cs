using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class EventDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime HoldingDate { get; set; }

        public EventTypes Type { get; set; }

        public int User { get; set; }

        public string Description { get; set; }
    }
}
