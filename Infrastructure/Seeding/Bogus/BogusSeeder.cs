using Infrastructure.Data;
using Infrastructure.Seeding.Bogus.AccountSeeding;
using Infrastructure.Seeding.Bogus.EmployeeSeeding;
using Infrastructure.Seeding.Bogus.StudentSeeding;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Infrastructure.Seeding.Bogus
{
    public class BogusSeeder
    {
        private readonly StopwatchService _stopwatchService;
        private readonly ILogger<BogusSeeder> _logger;

        public BogusSeeder(StopwatchService stopwatchService, ILogger<BogusSeeder> logger)
        {
            _stopwatchService = stopwatchService;
            _logger = logger;
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                _stopwatchService.Start();

                // Generate unique accounts
                var accounts = AccountSeeder.GenerateAccounts(SeedingConstants.AccountSeedCount);
                _stopwatchService.LogElapsed($"Generated {accounts.Count} accounts", "seconds");

                // Save accounts
                context.UsersAccounts.AddRange(accounts);
                context.SaveChanges();
                _stopwatchService.Stop();
                _stopwatchService.LogElapsed($"Generated and saved {accounts.Count} accounts", "seconds");

                // Generate Students
                _stopwatchService.Start();
                var students = StudentSeeder.GenerateStudents(accounts.Take(SeedingConstants.StudentSeedCount).ToList(), context);
                _stopwatchService.Stop();
                _stopwatchService.LogElapsed($"Generated and saved {students.Count} students", "seconds");

                // Generate Employees
                _stopwatchService.Start();
                var employees = EmployeeSeeder.GenerateEmployees(accounts.Skip(SeedingConstants.EmployeeSeedCount).Take(SeedingConstants.EmployeeSeedCount).ToList(), context);
                _stopwatchService.LogElapsed($"Generated {employees.Count} employees", "seconds");

                // Save employees
                context.Employees.AddRange(employees);
                context.SaveChanges();
                _stopwatchService.Stop();
                _stopwatchService.LogElapsed($"Generated and saved {employees.Count} employees", "seconds");
            }
        }
    }
}
