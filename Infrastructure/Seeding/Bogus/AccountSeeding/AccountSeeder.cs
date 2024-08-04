using Bogus;
using Domain.Entities.AccountEntities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeding.Bogus.AccountSeeding
{
    public class AccountSeeder
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;
        private readonly HashSet<string> _exisitngUsernames;

        public AccountSeeder(IAccountRepository accountRepository, IPasswordHasher<UserAccount> passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;

            // Load all exisitng usernames into memory for fast lookup
            _exisitngUsernames = new HashSet<string>(_accountRepository.GetAllUsernames());

        }

        public UserAccount GenerateAccountForPerson(string firstName, string lastName)
        {
            var account = new Faker<UserAccount>()
                .RuleFor(ua => ua.Id, f => Guid.NewGuid())
                .RuleFor(ua => ua.Login, f => GenerateUniqueLogin(firstName, lastName))
                .RuleFor(ua => ua.Password, f => GenerateHashedPassword(f.Internet.Password()))
                .RuleFor(ua => ua.Email, f => f.Internet.Email());

            return account;
        }
        private string GenerateUniqueLogin(string firstName, string lastName)
        {
            var baseLogin = $"{firstName.ToLower()}.{lastName.ToLower()}";
            var nextLogin = baseLogin;
            int counter = 1;

            // check in-memory collection for existing usernames
            while (_exisitngUsernames.Contains(nextLogin))
            {
                nextLogin = $"{baseLogin}{counter}";
                counter++;
            }

            //Add newly created unique login to the in-memory set
            _exisitngUsernames.Add(nextLogin);

            return nextLogin;
        }

        private string GenerateHashedPassword(string plainPassword)
        {
            var dummyUser = new UserAccount();
            return _passwordHasher.HashPassword(dummyUser, plainPassword);
        }
    }
}
