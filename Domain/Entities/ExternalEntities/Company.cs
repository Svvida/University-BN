using Domain.Entities.AccountEntities;
using Domain.EntitiesBase;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.ExternalEntities
{
    public class Company : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Address { get; set; }
        public Guid? AccountId { get; set; }
        public UserAccount? Account { get; set; }
        public ICollection<ExternalParticipantComanies> ExternalParticipantComanies { get; set; } = new List<ExternalParticipantComanies>();

        public Company() { }
        public Company(Guid id, string name, string address, Guid? accountId)
        {
            Id = id;
            Name = name;
            Address = address;
            AccountId = accountId;
        }
    }
}
