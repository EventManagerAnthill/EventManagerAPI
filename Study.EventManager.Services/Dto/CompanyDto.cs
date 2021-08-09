using System.ComponentModel.DataAnnotations;

namespace Study.EventManager.Services.Dto
{
    public class CompanyDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Type { get; set; }

        public string Description { get; set; }

        public int Del { get; set; }
    }
}
