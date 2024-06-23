using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserAccount
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();
        public Student? Student { get; set; }
        public Employee? Employee { get; set; }

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
