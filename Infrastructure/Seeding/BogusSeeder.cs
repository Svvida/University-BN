using Bogus;
using Domain.Entities.StudentEntities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Domain.Enums;
using Infrastructure.Seeding.AccountSeeding;
using Infrastructure.Seeding.StudentSeeding;
using Infrastructure.Seeding.EmployeeSeeding;

namespace Infrastructure.Seeding
{
    public class BogusSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                // Generate unique accounts
                var accounts = AccountSeeder.GenerateAccounts(200);
                context.UsersAccounts.AddRange(accounts);
                context.SaveChanges();

                // Generate Students
                var students = StudentSeeder.GenerateStudents(accounts.Take(100).ToList(), context);
                context.Students.AddRange(students);

                // Generate Employees
                var employees = EmployeeSeeder.GenerateEmployees(accounts.Skip(100).Take(100).ToList(), context);
                context.Employees.AddRange(employees);

                context.SaveChanges();
            }
        }
    }
}
