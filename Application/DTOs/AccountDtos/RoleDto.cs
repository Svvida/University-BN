using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDtos
{
    public class RoleDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
