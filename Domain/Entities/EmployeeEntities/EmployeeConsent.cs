using Domain.EntitiesBase;

namespace Domain.Entities.EmployeeEntities
{
    public class EmployeeConsent : ConsentBase
    {
        public Employee? Employee { get; set; }

        public EmployeeConsent() { }

        public EmployeeConsent(Guid id, bool permissionForPhoto, bool permissionForDataProcessing)
            : base(id, permissionForPhoto, permissionForDataProcessing) { }
    }
}
