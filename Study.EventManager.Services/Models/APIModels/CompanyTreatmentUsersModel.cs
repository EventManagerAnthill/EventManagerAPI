using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Services.Models.APIModels
{
    public class CompanyTreatmentUsersModel
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string[] Email { get; set; }
    }
}
