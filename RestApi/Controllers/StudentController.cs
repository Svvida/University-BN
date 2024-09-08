using Application.Interfaces;
using Domain.Entities.StudentEntities;
using Domain.Interfaces.InterfacesBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IPersonService<Student> _personService;
        private readonly IPersonRepository<Student> _personRepository;

        public StudentController(
            IPersonService<Student> personService,
            IPersonRepository<Student> personRepository)
        {
            _personService = personService;
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _personService.GetAllAsync();

            return Ok(students);
        }

        [HttpPost("students/me")]
        public async Task<IActionResult> GetLoggedUserInfo()
        {
            var userClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userClaim) || !Guid.TryParse(userClaim, out var accountId))
            {
                return Unauthorized("Invalid or missing user ID");
            }

            var student = await _personRepository.GetByAccountIdAsync(new Guid(userClaim));

            if (student is null)
            {
                return NotFound("Student not found");
            }

            return Ok(student);
        }
    }
}
