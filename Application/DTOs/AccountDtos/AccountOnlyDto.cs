using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDtos
{
    public abstract class AccountOnlyDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
