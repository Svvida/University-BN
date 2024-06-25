using Domain.EntitiesBase;

namespace Domain.Entities.StudentEntities
{
    public class StudentConsent : ConsentBase
    {
        public Student? Student { get; set; }

        public StudentConsent() { }

        public StudentConsent(Guid id, bool permissionForPhoto, bool permissionForDataProcessing)
            : base(id, permissionForPhoto, permissionForDataProcessing) { }
    }
}
