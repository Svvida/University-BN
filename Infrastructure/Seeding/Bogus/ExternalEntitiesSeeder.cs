using Bogus;
using Domain.Entities.ExternalEntities;
using Domain.Enums;
using Infrastructure.Seeding.Bogus.AccountSeeding;

namespace Infrastructure.Seeding.Bogus
{
    public class ExternalEntitiesSeeder
    {
        private readonly AccountSeeder _accountSeeder;

        public ExternalEntitiesSeeder(AccountSeeder accountSeeder)
        {
            _accountSeeder = accountSeeder;
        }

        public List<Company> GenerateCompanies(int count)
        {
            var companies = new List<Company>();

            for (int i = 0; i < count; i++)
            {
                var company = new Faker<Company>()
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Name, f => f.Company.CompanyName())
                    .RuleFor(c => c.Address, f => f.Address.FullAddress())
                    .Generate();

                company.Account = _accountSeeder.GenerateAccountForComapny(
                    company.Name,
                    RoleType.Company.ToString(),
                    RoleType.EventOrganizer.ToString());

                companies.Add(company);
            }

            return companies;
        }

        public List<ExternalParticipant> GenerateExternalParticipant(int count, List<Company> companies)
        {
            var externalParticipants = new List<ExternalParticipant>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var externalParticipant = new Faker<ExternalParticipant>()
                    .RuleFor(ep => ep.Id, f => Guid.NewGuid())
                    .RuleFor(ep => ep.Name, f => f.Person.FirstName)
                    .RuleFor(ep => ep.Surname, f => f.Person.LastName)
                    .RuleFor(ep => ep.ContactEmail, f => f.Person.Email)
                    .RuleFor(ep => ep.ContactPhone, f => f.Phone.PhoneNumberFormat())
                    .Generate();

                externalParticipant.Account = _accountSeeder.GenerateAccountForPerson(
                    externalParticipant.Name,
                    externalParticipant.Surname,
                    RoleType.ExternalParticipant.ToString());

                // Randomly pick three unique companies
                var assignedCompanies = new HashSet<Company>();
                while (assignedCompanies.Count < 3)
                {
                    var randomIndex = random.Next(companies.Count);
                    assignedCompanies.Add(companies[randomIndex]);
                }

                foreach (var company in assignedCompanies)
                {
                    externalParticipant.ExternalParticipantComanies.Add(new ExternalParticipantComanies
                    {
                        CompanyId = company.Id,
                        ExternalParticipantId = externalParticipant.Id
                    });
                }

                externalParticipants.Add(externalParticipant);
            }

            return externalParticipants;
        }
    }
}
