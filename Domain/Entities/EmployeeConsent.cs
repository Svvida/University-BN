using Domain.EntitiesBase;

namespace Domain.Entities
{
    internal class EmployeeConsent : ConsentBase
    {
        public Employee? Employee { get; set; }
    }
}
