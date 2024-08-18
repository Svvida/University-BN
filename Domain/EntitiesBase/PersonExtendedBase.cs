using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class PersonExtendedBase : PersonBase
    {
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; } = 0;
        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL must be 11 digits long.")]
        [RegularExpression(@"\d{11}", ErrorMessage = "PESEL must consist of 11 digits.")]
        public string PESEL { get; set; }

        [Required]
        public DateTime DateOfAddmission { get; set; }

        public Guid? AddressId { get; set; }
        public Guid? ConsentId { get; set; }

        protected PersonExtendedBase() : base()
        {
        }

        protected PersonExtendedBase(Guid id, string name, string surname, DateTime dateOfBirth, Gender gender, string pesel, string contactEmail, string contactPhone, DateTime dateOfAddmission, Guid? addressId, Guid? accountId, Guid? consentId)
            : base(id, name, surname, contactEmail, contactPhone, accountId)
        {
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PESEL = pesel;
            DateOfAddmission = dateOfAddmission;
            AddressId = addressId;
            ConsentId = consentId;
        }
    }
}
