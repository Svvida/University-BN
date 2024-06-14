using Domain.EntitiesBase;

namespace Domain.Entities
{
    internal class EmployeeAddress : AddressBase
    {
        public Employee? Employee { get; set; }
    }
}
