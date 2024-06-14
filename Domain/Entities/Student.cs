using Domain.EntitiesBase;

namespace Domain.Entities
{
    internal class Student : PersonBase
    {
        public StudentAddress? Address { get; set; }
        public StudentConsent? Consent { get; set; }
    }
}
