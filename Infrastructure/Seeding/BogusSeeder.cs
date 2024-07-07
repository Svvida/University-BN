using System;
using System.Linq;
using Bogus;
using Domain.Entities.StudentEntities;
using Infrastructure.Data;
using Infrastructure.Seeding.AccountSeeding;
using Infrastructure.Seeding.EmployeeSeeding;
using Infrastructure.Seeding.StudentSeeding;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace Infrastructure.Seeding
{
    public class BogusSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                StopwatchService.Instance.Start();

                // Generate unique accounts
                StopwatchService.Instance.LogElapsed("Starting account generation", "seconds");
                var accounts = AccountSeeder.GenerateAccounts(100000);
                StopwatchService.Instance.LogElapsed($"Generated {accounts.Count} accounts", "seconds");

                // Save accounts
                context.UsersAccounts.AddRange(accounts);
                context.SaveChanges();
                StopwatchService.Instance.LogElapsed($"Saved {accounts.Count} accounts", "seconds");

                // Generate Students
                StopwatchService.Instance.LogElapsed("Starting student generation", "seconds");
                var students = StudentSeeder.GenerateStudents(accounts.Take(50000).ToList(), context);
                StopwatchService.Instance.LogElapsed($"Generated {students.Count} students", "seconds");

                // Save students
                //context.Students.AddRange(students);
                //context.SaveChanges();
                StopwatchService.Instance.LogElapsed($"Saved {students.Count} students", "seconds");

                // Generate Employees
                StopwatchService.Instance.LogElapsed("Starting employee generation", "seconds");
                var employees = EmployeeSeeder.GenerateEmployees(accounts.Skip(50000).Take(50000).ToList(), context);
                StopwatchService.Instance.LogElapsed($"Generated {employees.Count} employees", "seconds");

                // Save employees
                context.Employees.AddRange(employees);
                context.SaveChanges();
                StopwatchService.Instance.LogElapsed($"Saved {employees.Count} employees", "seconds");

                StopwatchService.Instance.Stop();
            }
        }
    }
}
