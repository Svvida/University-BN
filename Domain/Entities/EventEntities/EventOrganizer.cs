using Domain.Entities.AccountEntities;
using Domain.EntitiesBase;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EventEntities
{
    public class EventOrganizer : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public EventOrganizerType OrganizerType { get; set; }
        public ICollection<EventOrganizerEvents> EventOrganizersEvents { get; set; } = new List<EventOrganizerEvents>();

        public UserAccount? Account { get; set; }

        public EventOrganizer() { }

        public EventOrganizer(Guid id, Guid accountId, EventOrganizerType organizerType)
        {
            Id = id;
            AccountId = accountId;
            OrganizerType = organizerType;
        }
    }
}
