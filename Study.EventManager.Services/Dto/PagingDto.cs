using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class PagingDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}
