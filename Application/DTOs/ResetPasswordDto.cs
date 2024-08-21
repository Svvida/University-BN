using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
