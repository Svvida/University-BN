using Domain.Entities.EmployeeEntities;
using Domain.Entities.EventEntities;
using Domain.Entities.ExternalEntities;
using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AccountEntities
{
    public class UserAccount : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        // Navigation properties
        public ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();
        public Student? Student { get; set; }
        public Employee? Employee { get; set; }
        public EventOrganizer? EventOrganizer { get; set; }
        public Company? Company { get; set; }
        public ExternalParticipant? ExternalParticipant { get; set; }

        // Refresh token properties
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public UserAccount() { }

        public UserAccount(Guid id, string login, string password, string email)
        {
            Id = id;
            Login = login;
            Password = password;
            Email = email;
        }
    }
}
