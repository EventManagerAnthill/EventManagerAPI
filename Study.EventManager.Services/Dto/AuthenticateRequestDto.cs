using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class AuthenticateRequestDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
