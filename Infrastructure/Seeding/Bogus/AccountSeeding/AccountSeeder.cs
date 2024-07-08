using Bogus;
using Domain.Entities.AccountEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Infrastructure.Seeding.Bogus.AccountSeeding
{
    public static class AccountSeeder
    {
        public static List<UserAccount> GenerateAccounts(int count)
        {
            var accounts = new List<UserAccount>();
            var faker = new Faker<UserAccount>()
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.Login, f => f.Internet.UserName())
                .RuleFor(a => a.Password, f => f.Internet.Password())
                .RuleFor(a => a.Email, f => f.Internet.Email());

            for (int i = 0; i < count; i++)
            {
                var account = faker.Generate();
                EnsureUniqueLogin(accounts, account);
                EnsureUniqueEmail(accounts, account);
                accounts.Add(account);
            }
            return accounts;
        }

        private static void EnsureUniqueLogin(List<UserAccount> accounts, UserAccount account)
        {
            var originalLogin = account.Login;
            var suffix = 1;
            while (accounts.Any(a => a.Login == account.Login))
            {
                account.Login = originalLogin + suffix;
                suffix++;
            }
        }

        private static void EnsureUniqueEmail(List<UserAccount> accounts, UserAccount account)
        {
            var originalEmail = account.Email;
            var atIndex = originalEmail.IndexOf("@");
            var prefix = originalEmail.Substring(0, atIndex);
            var domain = originalEmail.Substring(atIndex);
            var suffix = 1;
            while (accounts.Any(a => a.Email == account.Email))
            {
                account.Email = prefix + suffix + domain;
                suffix++;
            }
        }
    }
}
