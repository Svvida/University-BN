using Bogus;
using Domain.Entities.EventEntities;

namespace Infrastructure.Seeding.Bogus
{
    public class EventEntitiesSeeder
    {
        public EventEntitiesSeeder()
        {
        }

        public List<Event> GenerateEvents(int count, List<EventOrganizer> eventOrganizers)
        {
            var events = new List<Event>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var eventEntity = new Faker<Event>()
                    .RuleFor(e => e.Id, f => Guid.NewGuid())
                    .RuleFor(e => e.StartDate, f => f.Date.Soon())
                    .RuleFor(e => e.EndDate, (f, e) => f.Date.Soon(10, e.StartDate))
                    .RuleFor(e => e.Description, f => f.Lorem.Paragraph())
                    .Generate();

                // Assign random EventOrganizers to this event
                var organizersForEvent = eventOrganizers
                    .Take(random.Next(1, 4))
                    .ToList();

                foreach (var organizer in organizersForEvent)
                {
                    var eventOrganizerEvent = new EventOrganizerEvents
                    {
                        EventId = eventEntity.Id,
                        EventOrganizerId = organizer.Id
                    };
                    eventEntity.EventOrganizersEvents.Add(eventOrganizerEvent);
                }

                events.Add(eventEntity);
            }

            return events;
        }
    }
}
