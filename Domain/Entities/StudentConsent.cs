using Domain.EntitiesBase;
using System;

namespace Domain.Entities
{
    public class StudentConsent : ConsentBase
    {
        public Student? Student { get; set; }

        public StudentConsent() { }

        public StudentConsent(Guid id, bool permissionForPhoto, bool permissionForDataProcessing)
            : base(id, permissionForPhoto, permissionForDataProcessing) { }
    }
}
