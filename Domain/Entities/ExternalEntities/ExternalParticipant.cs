using Domain.Entities.AccountEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ExternalEntities
{
    public class ExternalParticipant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surename {  get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Phone]
        public string ContactPhone { get; set; }
        public Guid? AccountId {  get; set; }
        public UserAccount Account { get; set; }


        public ExternalParticipant() { }

        public ExternalParticipant(Guid id, string name, string contactEmail, string contactPhone, Guid? accountId)
        {
            Id = id;
            Name = name;
            ContactEmail = contactEmail;
            ContactPhone = contactPhone;
            AccountId = accountId;
        }
    }
}
