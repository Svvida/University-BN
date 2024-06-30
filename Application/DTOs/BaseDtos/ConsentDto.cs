using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BaseDtos
{
    public abstract class ConsentDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool PermissionForPhoto { get; set; }
        [Required]
        public bool PermissionForDataProcessing { get; set; }
    }
}
