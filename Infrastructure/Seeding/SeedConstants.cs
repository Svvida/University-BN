using Domain.Entities.AccountEntities;
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
                var roles = new List<Role>
                {
                    new Role { Id = SeedingConstants.AdminRoleId, Name = SeedingConstants.AdminRoleName, NormalizedName = SeedingConstants.AdminRoleName.ToUpper() },
                    new Role { Id = SeedingConstants.TeacherRoleId, Name = SeedingConstants.TeacherRoleName, NormalizedName = SeedingConstants.TeacherRoleName.ToUpper() },
                    new Role { Id = SeedingConstants.StudentRoleId, Name = SeedingConstants.StudentRoleName, NormalizedName = SeedingConstants.StudentRoleName.ToUpper() }
                };

                context.Roles.AddRange(roles);
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

                var adminRole = context.Roles.FirstOrDefault(r => r.NormalizedName == SeedingConstants.AdminRoleName.ToUpper());
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
