using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();

        public Role() { }

        public Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
