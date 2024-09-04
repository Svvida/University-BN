using Application.DTOs.AccountDtos;
using Application.DTOs.Employee.Dtos;
using Application.DTOs.Student.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account.Dtos
{
    public class AccountOnlyDto
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

    public class AccountFullDto : AccountOnlyDto
    {
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

        public Guid? StudentId { get; set; }
        public StudentOnlyDto Student { get; set; }

        public Guid? EmployeeId { get; set; }
        public EmployeeOnlyDto Employee { get; set; }
    }

    public class AccountCreateDto : AccountOnlyDto
    {
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }

    public class AccountUpdateDto : AccountOnlyDto
    {
        public string Password { get; set; }
    }

    public class UpdatePasswordDto
    {
        public Guid AccountId { get; set; }
        public string NewPassword { get; set; }
    }
}
