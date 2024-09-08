using Domain.Entities.EventEntities;
using Domain.Interfaces.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/")]
    public class EventController : ControllerBase
    {
        private readonly ICRUDRepository<Event> _eventRepository;

        public EventController(ICRUDRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet("community/events")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventRepository.GetAllAsync();

            return Ok(events);
        }
    }
}
