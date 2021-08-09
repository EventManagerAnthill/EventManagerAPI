using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class CompanyCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Type { get; set; }

        public string Description { get; set; }
    }
}
