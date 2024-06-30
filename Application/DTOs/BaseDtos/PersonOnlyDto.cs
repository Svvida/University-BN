using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BaseDtos
{
    public abstract class PersonOnlyDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; } = 0;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Required]

        [MaxLength(15)]
        public string ContactPhone { get; set; }

        [Required]
        public DateTime DateOfAddmission { get; set; }
    }
}
