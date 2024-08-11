using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class PersonBase : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContactPhone { get; set; }

        public Guid? AccountId { get; set; }

        protected PersonBase()
        {
        }

        protected PersonBase(Guid id, string name, string surname, string contactEmail, string contactPhone, Guid? accountId)
        {
            Id = id;
            Name = name;
            Surname = surname;
            ContactEmail = contactEmail;
            ContactPhone = contactPhone;
            AccountId = accountId;
        }
    }
}
