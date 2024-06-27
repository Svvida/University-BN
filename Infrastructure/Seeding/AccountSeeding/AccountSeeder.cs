using Bogus;
using Domain.Entities.AccountEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeding.AccountSeeding
{
    public static class AccountSeeder
    {
        public static List<UserAccount> GenerateAccounts(int count)
        {
            var accounts = new Faker<UserAccount>()
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.Login, f => f.Internet.UserName())
                .RuleFor(a => a.Password, f => f.Internet.Password())
                .RuleFor(a => a.Email, f => f.Internet.Email());

            return accounts.Generate(count);
        }
    }
}
