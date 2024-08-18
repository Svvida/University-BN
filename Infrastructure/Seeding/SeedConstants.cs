using Domain.Entities.AccountEntities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeding
{
    public static class SeedConstants
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<UniversityContext>();
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<UserAccount>>();

            SeedRoles(context);
            SeedAdminAccount(context, passwordHasher);
        }

        private static void SeedRoles(UniversityContext context)
        {
            if (!context.Roles.Any())
            {
                foreach (var role in Enum.GetValues(typeof(RoleType)).Cast<RoleType>())
                {
                    var roleName = role.ToString();
                    context.Roles.Add(new Role
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    });
                }

                context.SaveChanges();
            }
        }

        private static void SeedAdminAccount(UniversityContext context, IPasswordHasher<UserAccount> passwordHasher)
        {
            if (!context.UsersAccounts.Any(u => u.Login == SeedingConstants.AdminLogin))
            {
                var adminAccount = new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Login = SeedingConstants.AdminLogin,
                    Email = SeedingConstants.AdminEmail,
                    Password = passwordHasher.HashPassword(null, SeedingConstants.AdminPassword)
                };

                var adminRole = context.Roles.FirstOrDefault(r => r.NormalizedName == RoleType.Admin.ToString().ToUpper());
                if (adminRole != null)
                {
                    adminAccount.UserAccountRoles.Add(new UserAccountRole { RoleId = adminRole.Id });
                }

                context.UsersAccounts.Add(adminAccount);
                context.SaveChanges();
            }
        }
    }
}
