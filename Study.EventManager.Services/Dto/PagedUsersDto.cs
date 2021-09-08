using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class PagedUsersDto
    {
        public List<UserDto> Users { get; set; }
        public PagingDto Paging { get; set; }
    }
}
