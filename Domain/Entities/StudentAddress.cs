using Domain.EntitiesBase;

namespace Domain.Entities
{
    internal class StudentAddress : AddressBase
    {
        public Student? Student { get; set; }
    }
}
