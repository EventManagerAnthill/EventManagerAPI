using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Services.Dto
{
    public class PagedCompaniesDto
    {
        public List<CompanyDto> Companies { get; set; }
        public PagingDto Paging { get; set; }
    }
}
