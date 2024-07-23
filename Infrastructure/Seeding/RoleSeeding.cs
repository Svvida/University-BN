using Domain.Entities.AccountEntities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeding
{
    public class RoleSeeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            var context = serviceProvider.GetRequiredService<UniversityContext>();

            var numberOfRolesInDb = context.Roles.Count();

            if (numberOfRolesInDb == 0)
            {
                // Generate roles
                var roles = new List<Role>
                {
                    new Role { Id = SeedingConstants.AdminRoleId, Name = SeedingConstants.AdminRoleName, NormalizedName = SeedingConstants.AdminRoleName.ToUpper() },
                    new Role { Id = SeedingConstants.TeacherRoleId, Name = SeedingConstants.TeacherRoleName, NormalizedName = SeedingConstants.TeacherRoleName.ToUpper() },
                    new Role { Id = SeedingConstants.StudentRoleId, Name = SeedingConstants.StudentRoleName, NormalizedName = SeedingConstants.StudentRoleName.ToUpper() }
                };

                // Save roles
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
