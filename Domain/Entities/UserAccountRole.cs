using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserAccountRole
    {

        [Required]
        public Guid AccountId { get; set; }
        public UserAccount? Account { get; set; }

        [Required]
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }

        public UserAccountRole() { }

        public UserAccountRole(Guid accountId, Guid roleId)
        {
            AccountId = accountId;
            RoleId = roleId;
        }
    }
}
