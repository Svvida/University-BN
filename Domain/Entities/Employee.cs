using Domain.EntitiesBase;

namespace Domain.Entities
{
    internal class Employee : PersonBase
    {
        public EmployeeAddress? Address { get; set; }
        public EmployeeConsent? Consent { get; set; }
    }
}
