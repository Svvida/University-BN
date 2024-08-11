using Domain.Entities.AccountEntities;
using Domain.EntitiesBase;

namespace Domain.Entities.ExternalEntities
{
    public class ExternalParticipant : PersonBase
    {
        public UserAccount Account { get; set; }
        public ICollection<ExternalParticipantComanies> ExternalParticipantComanies { get; set; } = new List<ExternalParticipantComanies>();

        public ExternalParticipant() : base() { }

        public ExternalParticipant(Guid id, string name, string surname, string contactEmail, string contactPhone, Guid? accountId)
            : base(id, name, surname, contactEmail, contactPhone, accountId)
        {
        }
    }
}
