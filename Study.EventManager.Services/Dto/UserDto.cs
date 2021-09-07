using Study.EventManager.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class UserDto
    { 
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public UserSex Sex { get; set; }

        public string Username { get; set; }

        public string OriginalFileName { get; set; }

        public string ServerFileName { get; set; }

        public string FotoUrl { get; set; }

        public UserRoleEnum Role { get; set; }
    }
}
