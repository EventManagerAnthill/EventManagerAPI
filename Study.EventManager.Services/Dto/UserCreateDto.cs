using System;
using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class UserCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool EmailVerification { get; set; }
    }
}
