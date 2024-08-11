using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EventEntities
{
    public class EventOrganizerEvents
    {
        [Key]
        public Guid EventOrganizerId { get; set; }
        public EventOrganizer? EventOrganizer { get; set; }
        [Key]
        public Guid EventId { get; set; }
        public Event? Event { get; set; }

        public EventOrganizerEvents() { }
        public EventOrganizerEvents(Guid eventOrganizerId, Guid eventId)
        {
            EventOrganizerId = eventOrganizerId;
            EventId = eventId;
        }
    }
}
