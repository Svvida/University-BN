using Domain.EntitiesBase;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EventEntities
{
    public class Event : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }
        public ICollection<EventOrganizerEvents> EventOrganizersEvents { get; set; } = new List<EventOrganizerEvents>();

        public Event() { }

        public Event(Guid id, DateTime startDate, DateTime endDate, string description)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
    }
}
