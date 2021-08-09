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

        public string Middlename { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public UserSex Sex { get; set; }

        public string Username { get; set; }
    }
}
