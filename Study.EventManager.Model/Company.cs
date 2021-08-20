using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Model
{
    public class Company
    {
        internal Company()
        { }
        public Company(string companyName, int companyUserId, int companyType, string companyDescription)
        {
            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(companyUserId.ToString()) || string.IsNullOrEmpty(companyType.ToString()) || string.IsNullOrEmpty(companyDescription))
                throw new ArgumentNullException("Fill all required fields");

            Name = companyName;
            UserId = companyUserId;
            Type = companyType;
            Description = companyDescription;
            Del = 0;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int Type { get; set; }

        public string Description { get; set; }

        public int Del { get; set; }

        public virtual List<User> Users { get; set; } = new List<User>();

        public string OriginalFileName { get; set; }

        public string ServerFileName { get; set; }

        public string FotoUrl { get; set; }
    }
}
