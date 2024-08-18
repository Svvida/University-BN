using Application.Interfaces;
using Domain.Entities.EmployeeEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("/")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IPersonService<Employee> _personService;

        public EmployeeController(IPersonService<Employee> personService)
        {
            _personService = personService;
        }

        [HttpGet("api/community")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var employees = await _personService.GetAllAsync();

            return Ok(employees);
        }
    }
}
