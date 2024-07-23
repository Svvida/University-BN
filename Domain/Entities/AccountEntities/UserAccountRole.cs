using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AccountEntities
{
    public class UserAccountRole
    {

        [Required]
        public Guid AccountId { get; set; }
        public UserAccount? Account { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public UserAccountRole() { }

        public UserAccountRole(Guid accountId, int roleId)
        {
            AccountId = accountId;
            RoleId = roleId;
        }
    }
}
