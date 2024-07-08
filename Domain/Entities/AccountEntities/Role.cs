using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AccountEntities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string NormalizedName { get; set; }
        public ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();

        public Role() { }

        public Role(int id, string name, string normalizedName)
        {
            Id = id;
            Name = name;
            NormalizedName = normalizedName;
        }
    }
}
