using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BaseDtos
{
    public abstract class EducationDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
    }
}
