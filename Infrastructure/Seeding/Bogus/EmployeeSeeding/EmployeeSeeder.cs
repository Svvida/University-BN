using Bogus;
using Bogus.Extensions.Poland;
using Domain.Entities.EmployeeEntities;
using Domain.Enums;
using Infrastructure.Seeding.Bogus.AccountSeeding;

namespace Infrastructure.Seeding.Bogus.EmployeeSeeding
{
    public class EmployeeSeeder
    {
        private readonly AccountSeeder _accountSeeder;

        public EmployeeSeeder(AccountSeeder accountSeeder)
        {
            _accountSeeder = accountSeeder;
        }

        public List<Employee> GenerateEmployees(int count)
        {
            var employees = new List<Employee>();

            for (int i = 0; i < count; i++)
            {
                var employee = new Faker<Employee>()
                    .RuleFor(s => s.Id, f => Guid.NewGuid())
                    .RuleFor(s => s.Name, f => f.Name.FirstName())
                    .RuleFor(s => s.Surname, f => f.Name.LastName())
                    .RuleFor(s => s.DateOfBirth, f => f.Date.Past(20, DateTime.Now.AddYears(-18)))
                    .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                    .RuleFor(s => s.PESEL, f => f.Person.Pesel())
                    .RuleFor(s => s.ContactEmail, f => f.Internet.Email())
                    .RuleFor(s => s.ContactPhone, f => f.Phone.PhoneNumberFormat())
                    .RuleFor(s => s.DateOfAddmission, f => f.Date.Past(3))
                    .Generate();

                // Generate and associate account with employee
                employee.Account = _accountSeeder.GenerateAccountForPerson(
                    employee.Name,
                    employee.Surname,
                    RoleType.Teacher.ToString(),
                    RoleType.EventOrganizer.ToString());

                // Add additional related entities
                employee.Address = GenerateAddress();
                employee.Consent = GenerateConsent();

                employees.Add(employee);
            }

            return employees;
        }
        private EmployeeAddress GenerateAddress()
        {
            return new Faker<EmployeeAddress>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Country, f => f.Address.Country())
                .RuleFor(s => s.City, f => f.Address.City())
                .RuleFor(s => s.PostalCode, f => f.Address.ZipCode())
                .RuleFor(s => s.Street, f => f.Address.StreetAddress())
                .RuleFor(s => s.BuildingNumber, f => f.Address.BuildingNumber())
                .RuleFor(s => s.ApartmentNumber, f => f.Random.Bool() ? f.Address.BuildingNumber() : null)
                .Generate();
        }

        private EmployeeConsent GenerateConsent()
        {
            return new Faker<EmployeeConsent>()
                .RuleFor(sc => sc.Id, f => Guid.NewGuid())
                .RuleFor(sc => sc.PermissionForPhoto, f => f.Random.Bool())
                .RuleFor(sc => sc.PermissionForDataProcessing, f => f.Random.Bool())
                .Generate();
        }
    }
}
