using Domain.Entities;
using Domain.Enums;

namespace Domain.EntitiesBase
{
    internal class PersonBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; } = 0;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public DateTime DateOfAddmission { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? AccountId { get; set; }
        public UserAccount? Account { get; set; }
        public Guid? ConsentId { get; set; }
    }
}
