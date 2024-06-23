using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Enums;

namespace Domain.EntitiesBase
{
    public abstract class PersonBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; } = 0;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [MaxLength(15)]
        public string ContactPhone { get; set; }

        [Required]
        public DateTime DateOfAddmission { get; set; }

        public Guid? AddressId { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ConsentId { get; set; }

        protected PersonBase()
        {
        }

        protected PersonBase(Guid id, string name, string surname, DateTime dateOfBirth, Gender gender, string contactEmail, string contactPhone, DateTime dateOfAddmission, Guid? addressId, Guid? accountId, Guid? consentId)
        {
            Id = id;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            ContactEmail = contactEmail;
            ContactPhone = contactPhone;
            DateOfAddmission = dateOfAddmission;
            AddressId = addressId;
            AccountId = accountId;
            ConsentId = consentId;
        }
    }
}
