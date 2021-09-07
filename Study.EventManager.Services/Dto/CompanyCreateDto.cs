using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class CompanyCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Type { get; set; }

        public string Description { get; set; }
    }
}
