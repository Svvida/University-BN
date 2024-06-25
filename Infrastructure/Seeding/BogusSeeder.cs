using Bogus;
using Domain.Entities.StudentEntities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeding
{
    public class BogusSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                if (!context.Students.Any())
                {
                    var students = new Faker<Student>()
                        .RuleFor(s => s.Id, f => Guid.NewGuid())
                        .RuleFor(s => s.Name, f => f.Name.FirstName());
                }
            }
        }
    }
}
