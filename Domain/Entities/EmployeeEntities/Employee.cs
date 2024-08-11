using Domain.Entities.AccountEntities;
using Domain.EntitiesBase;
using Domain.Enums;

namespace Domain.Entities.EmployeeEntities
{
    public class Employee : PersonExtendedBase
    {
        public EmployeeAddress? Address { get; set; }
        public EmployeeConsent? Consent { get; set; }
        public UserAccount? Account { get; set; }

        public Employee()
        {
        }

        public Employee(Guid id, string name, string surname, DateTime dateOfBirth, Gender gender, string pesel, string contactEmail, string contactPhone, DateTime dateOfAddmission, Guid? addressId, Guid? accountId, Guid? consentId)
            : base(id, name, surname, dateOfBirth, gender, pesel, contactEmail, contactPhone, dateOfAddmission, addressId, accountId, consentId)
        {
        }
    }
}
