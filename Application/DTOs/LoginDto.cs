using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Identifier { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
