using Bogus;
using Domain.Entities.AccountEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.StudentEntities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Infrastructure.Seeding.EmployeeSeeding
{
    public static class EmployeeSeeder
    {
        public static List<Employee> GenerateEmployees(List<UserAccount> accounts, UniversityContext context)
        {
            var addresses = GenerateAddresses(100);
            var consents = GenerateConsents(100);

            context.EmployeesAddresses.AddRange(addresses);
            context.EmployeesConsents.AddRange(consents);
            context.SaveChanges();

            var employees = new Faker<Employee>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Name, f => f.Name.FirstName())
                .RuleFor(s => s.Surname, f => f.Name.LastName())
                .RuleFor(s => s.DateOfBirth, f => f.Date.Past(20, DateTime.Now.AddYears(-18)))
                .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                .RuleFor(s => s.ContactEmail, f => f.Internet.Email())
                .RuleFor(s => s.ContactPhone, f => f.Phone.PhoneNumberFormat())
                .RuleFor(s => s.DateOfAddmission, f => f.Date.Past(3))
                .RuleFor(s => s.AddressId, (f, s) => addresses[f.IndexFaker].Id)
                .RuleFor(s => s.AccountId, (f, s) => accounts[f.IndexFaker].Id)
                .RuleFor(s => s.ConsentId, (f, s) => consents[f.IndexFaker].Id);

            return employees.Generate(accounts.Count);
        }
        private static List<EmployeeAddress> GenerateAddresses(int count)
        {
            var addresses = new Faker<EmployeeAddress>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Country, f => f.Address.Country())
                .RuleFor(s => s.City, f => f.Address.City())
                .RuleFor(s => s.PostalCode, f => f.Address.ZipCode())
                .RuleFor(s => s.Street, f => f.Address.StreetAddress())
                .RuleFor(s => s.BuildingNumber, f => f.Address.BuildingNumber())
                .RuleFor(s => s.ApartmentNumber, f => f.Random.Bool() ? f.Address.SecondaryAddress() : null);

            return addresses.Generate(count);
        }

        private static List<EmployeeConsent> GenerateConsents(int count)
        {
            var consents = new Faker<EmployeeConsent>()
                .RuleFor(sc => sc.Id, f => Guid.NewGuid())
                .RuleFor(sc => sc.PermissionForPhoto, f => f.Random.Bool())
                .RuleFor(sc => sc.PermissionForDataProcessing, f => f.Random.Bool());

            return consents.Generate(count);
        }
    }
}
