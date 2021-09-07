using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class PagedEventsDto
    {
        public List<EventDto> Events { get; set; }
        public PagingDto Paging { get; set; }
    }
}
