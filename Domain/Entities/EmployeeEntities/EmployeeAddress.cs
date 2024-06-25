using Domain.EntitiesBase;

namespace Domain.Entities.EmployeeEntities
{
    public class EmployeeAddress : AddressBase
    {
        public Employee? Employee { get; set; }

        public EmployeeAddress() { }

        public EmployeeAddress(Guid id, string country, string city, string postalCode, string street, string buildingNumber, string? apartmentNumber)
            : base(id, country, city, postalCode, street, buildingNumber, apartmentNumber) { }
    }
}
