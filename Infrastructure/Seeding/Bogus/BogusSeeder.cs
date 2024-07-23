using Infrastructure.Data;
using Infrastructure.Seeding.Bogus.AccountSeeding;
using Infrastructure.Seeding.Bogus.EmployeeSeeding;
using Infrastructure.Seeding.Bogus.StudentSeeding;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace Infrastructure.Seeding.Bogus
{
    public class BogusSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                StopwatchService.Instance.Start();

                // Generate unique accounts
                var accounts = AccountSeeder.GenerateAccounts(SeedingConstants.AccountSeedCount);
                StopwatchService.Instance.LogElapsed($"Generated {accounts.Count} accounts", "seconds");

                // Save accounts
                context.UsersAccounts.AddRange(accounts);
                context.SaveChanges();
                StopwatchService.Instance.Stop();
                StopwatchService.Instance.LogElapsed($"Generated and saved {accounts.Count} accounts", "seconds");

                // Generate Students
                StopwatchService.Instance.Start();
                var students = StudentSeeder.GenerateStudents(accounts.Take(SeedingConstants.StudentSeedCount).ToList(), context);
                StopwatchService.Instance.Stop();
                StopwatchService.Instance.LogElapsed($"Generated and saved {students.Count} students", "seconds");

                // Generate Employees
                StopwatchService.Instance.Start();
                var employees = EmployeeSeeder.GenerateEmployees(accounts.Skip(SeedingConstants.EmployeeSeedCount).Take(SeedingConstants.EmployeeSeedCount).ToList(), context);
                StopwatchService.Instance.LogElapsed($"Generated {employees.Count} employees", "seconds");

                // Save employees
                context.Employees.AddRange(employees);
                context.SaveChanges();
                StopwatchService.Instance.Stop();
                StopwatchService.Instance.LogElapsed($"Generated and saved {employees.Count} employees", "seconds");

            }
        }
    }
}
