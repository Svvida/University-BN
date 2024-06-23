using Domain.EntitiesBase;
using System;

namespace Domain.Entities
{
    public class EmployeeConsent : ConsentBase
    {
        public Employee? Employee { get; set; }

        public EmployeeConsent() { }

        public EmployeeConsent(Guid id, bool permissionForPhoto, bool permissionForDataProcessing)
            : base(id, permissionForPhoto, permissionForDataProcessing) { }
    }
}
