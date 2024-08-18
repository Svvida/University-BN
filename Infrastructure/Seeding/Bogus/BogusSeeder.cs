using Domain.Entities.AccountEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.EventEntities;
using Domain.Entities.ExternalEntities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Seeding.Bogus.AccountSeeding;
using Infrastructure.Seeding.Bogus.EmployeeSeeding;
using Infrastructure.Seeding.Bogus.StudentSeeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UniversityContext _context;

        public BogusSeeder(
            StopwatchService stopwatchService,
            ILogger<BogusSeeder> logger,
            IPasswordHasher<UserAccount> passwordHasher,
            IAccountRepository accountRepository,
            UniversityContext context)
        {
            _stopwatchService = stopwatchService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _accountRepository = accountRepository;
            _context = context;
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            _stopwatchService.Start();

            // Initialize seeders
            var accountSeeder = new AccountSeeder(_accountRepository, _passwordHasher, _context);
            var studentSeeder = new StudentSeeder(accountSeeder);
            var employeeSeeder = new EmployeeSeeder(accountSeeder);
            var externalEntitiesSeeder = new ExternalEntitiesSeeder(accountSeeder);
            var eventEntitiesSeeder = new EventEntitiesSeeder();

            // Generate Students and related data
            var students = studentSeeder.GenerateStudents(SeedingConstants.StudentSeedCount);

            // Retrieve all necessary data for enrollment
            var degreeCourses = _context.DegreeCourses
                .Include(dc => dc.Paths)
                .ThenInclude(p => p.Modules)
                .ToList();

            // Enroll students in courses, paths, modules
            studentSeeder.EnrollStudentsInCourses(students, degreeCourses);
            studentSeeder.EnrollStudentsInPaths(students, degreeCourses);
            studentSeeder.EnrollStudentsInModlues(students, degreeCourses);

            // Save all data
            _context.Students.AddRange(students);
            _context.SaveChanges();
            _stopwatchService.LogElapsed($"Generated and saved {students.Count} students and related entities", "seconds");

            _stopwatchService.Stop();

            _stopwatchService.Start();

            // Generate Employees and related data
            var employees = employeeSeeder.GenerateEmployees(SeedingConstants.EmployeeSeedCount);
            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            // Generate Companies and related data
            var companies = externalEntitiesSeeder.GenerateCompanies(SeedingConstants.CompanySeedCount);
            _context.Companies.AddRange(companies);
            _context.SaveChanges();

            // Generate External Participants and related data
            var externalParticipants = externalEntitiesSeeder.GenerateExternalParticipant(SeedingConstants.ExternalParticipantSeedCount, companies);
            _context.ExternalParticipants.AddRange(externalParticipants);
            _context.SaveChanges();

            // Save all data
            _context.SaveChanges();
            _stopwatchService.LogElapsed($"Generated and saved employees, companies, and external participants", "seconds");

            _stopwatchService.Stop();

            // Separate step for seeding EventOrganizers
            _stopwatchService.Start();

            SeedEventOrganizers(employees, companies, externalParticipants);

            _stopwatchService.LogElapsed("Generated and saved event organizers", "seconds");

            _stopwatchService.Stop();

            // Separate step for seeding Events
            _stopwatchService.Start();

            var eventOrganizers = _context.EventOrganizers.ToList();
            var events = eventEntitiesSeeder.GenerateEvents(SeedingConstants.EventSeedCount, eventOrganizers);

            _context.Events.AddRange(events);
            _context.SaveChanges();

            _stopwatchService.LogElapsed("Generated and saved events", "seconds");

            _stopwatchService.Stop();
        }

        private void SeedEventOrganizers(List<Employee> employees, List<Company> companies, List<ExternalParticipant> externalParticipants)
        {
            // Create EventOrganizers for Employees
            foreach (var employee in employees)
            {
                var eventOrganizer = new EventOrganizer(Guid.NewGuid(), employee.Account.Id, EventOrganizerType.Employee);
                _context.EventOrganizers.Add(eventOrganizer);
            }

            // Create EventOrganizers for Companies
            foreach (var company in companies)
            {
                var eventOrganizer = new EventOrganizer(Guid.NewGuid(), company.Account.Id, EventOrganizerType.Company);
                _context.EventOrganizers.Add(eventOrganizer);
            }

            // Create EventOrganizers for ExternalParticipants
            foreach (var externalParticipant in externalParticipants)
            {
                var eventOrganizer = new EventOrganizer(Guid.NewGuid(), externalParticipant.Account.Id, EventOrganizerType.ExternalParticipant);
                _context.EventOrganizers.Add(eventOrganizer);
            }

            _context.SaveChanges();
        }
    }
}
