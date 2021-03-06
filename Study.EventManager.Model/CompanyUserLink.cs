using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Model
{
    public class CompanyUserLink
    {
        internal CompanyUserLink()
        { }

        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public int UserCompanyRole { get; set; }
    }
}
