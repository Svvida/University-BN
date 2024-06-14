using Domain.EntitiesBase;

namespace Domain.Entities
{
    internal class StudentConsent : ConsentBase
    {
        public Student? Student { get; set; }
    }
}
