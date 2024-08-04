using Domain.Entities.AccountEntities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Seeding.Bogus.AccountSeeding;
using Infrastructure.Seeding.Bogus.EmployeeSeeding;
using Infrastructure.Seeding.Bogus.StudentSeeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Infrastructure.Seeding.Bogus
{
    public class BogusSeeder
    {
        private readonly StopwatchService _stopwatchService;
        private readonly ILogger<BogusSeeder> _logger;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;
        private readonly IAccountRepository _accountRepository;

        public BogusSeeder(
            StopwatchService stopwatchService,
            ILogger<BogusSeeder> logger,
            IPasswordHasher<UserAccount> passwordHasher,
            IAccountRepository accountRepository)
        {
            _stopwatchService = stopwatchService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _accountRepository = accountRepository;
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                _stopwatchService.Start();

                // Initialize seeders
                var accountSeeder = new AccountSeeder(_accountRepository, _passwordHasher);
                var studentSeeder = new StudentSeeder(accountSeeder);
                var employeeSeeder = new EmployeeSeeder(accountSeeder);

                // Generate Students and related data
                var students = studentSeeder.GenerateStudents(SeedingConstants.StudentSeedCount);

                // Retrieve all necessary data for enrollment
                var degreeCourses = context.DegreeCourses
                    .Include(dc => dc.Paths)
                    .ThenInclude(p => p.Modules)
                    .ToList();

                // Enroll students in courses, paths, modules
                studentSeeder.EnrollStudentsInCourses(students, degreeCourses);
                studentSeeder.EnrollStudentsInPaths(students, degreeCourses);
                studentSeeder.EnrollStudentsInModlues(students, degreeCourses);

                // Save all data
                context.Students.AddRange(students);
                context.SaveChanges();
                _stopwatchService.LogElapsed($"Generated and saved {students.Count} students and related entities", "seconds");

                _stopwatchService.Stop();

                _stopwatchService.Start();

                // Generate Employees and related data
                var employees = employeeSeeder.GenerateEmployees(SeedingConstants.EmployeeSeedCount);

                // Save employees and related data
                context.Employees.AddRange(employees);
                context.SaveChanges();
                _stopwatchService.LogElapsed($"Generated and saved {employees.Count} employees and related entities", "seconds");

                _stopwatchService.Stop();
            }
        }
    }
}
