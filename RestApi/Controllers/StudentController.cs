using Application.Interfaces;
using Domain.Entities.StudentEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IPersonService<Student> _personService;

        public StudentController(IPersonService<Student> personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _personService.GetAllAsync();

            return Ok(students);
        }
    }
}
