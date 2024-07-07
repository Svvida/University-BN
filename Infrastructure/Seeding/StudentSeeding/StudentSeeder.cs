using Bogus;
using Domain.Entities.StudentEntities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Entities.AccountEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Utilities;

namespace Infrastructure.Seeding.StudentSeeding
{
    public static class StudentSeeder
    {
        public static List<Student> GenerateStudents(List<UserAccount> accounts, UniversityContext context)
        {

            var addresses = GenerateAddresses(5000);
            var consents = GenerateConsents(5000);

            context.StudentsAddresses.AddRange(addresses);
            context.StudentsConsents.AddRange(consents);
            context.SaveChanges();

            var students = new Faker<Student>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Name, f => f.Name.FirstName())
                .RuleFor(s => s.Surname, f => f.Name.LastName())
                .RuleFor(s => s.DateOfBirth, f => f.Date.Past(20, DateTime.Now.AddYears(-18)))
                .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                .RuleFor(s => s.ContactEmail, f => f.Internet.Email())
                .RuleFor(s => s.ContactPhone, f => f.Phone.PhoneNumberFormat())
                .RuleFor(s => s.DateOfAddmission, f => f.Date.Past(3))
                .RuleFor(s => s.AddressId, (f, s) => addresses[f.IndexFaker % addresses.Count].Id)
                .RuleFor(s => s.AccountId, (f, s) => accounts[f.IndexFaker % accounts.Count].Id)
                .RuleFor(s => s.ConsentId, (f, s) => consents[f.IndexFaker % consents.Count].Id);

            var generatedStudents = students.Generate(accounts.Count);

            context.Students.AddRange(generatedStudents);
            context.SaveChanges();

            EnrollStudentsInCourses(context, generatedStudents);
            context.SaveChanges();

            EnrollStudentsInPaths(context, generatedStudents);
            context.SaveChanges();

            EnrollStudentsInModlues(context, generatedStudents);
            context.SaveChanges();

            return generatedStudents;
        }

        private static List<StudentAddress> GenerateAddresses(int count)
        {
            var addresses = new Faker<StudentAddress>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Country, f => f.Address.Country())
                .RuleFor(s => s.City, f => f.Address.City())
                .RuleFor(s => s.PostalCode, f => f.Address.ZipCode())
                .RuleFor(s => s.Street, f => f.Address.StreetAddress())
                .RuleFor(s => s.BuildingNumber, f => f.Address.BuildingNumber())
                .RuleFor(s => s.ApartmentNumber, f => f.Random.Bool() ? f.Address.BuildingNumber() : null);

            return addresses.Generate(count);
        }

        private static List<StudentConsent> GenerateConsents(int count)
        {
            var consents = new Faker<StudentConsent>()
                .RuleFor(sc => sc.Id, f => Guid.NewGuid())
                .RuleFor(sc => sc.PermissionForPhoto, f => f.Random.Bool())
                .RuleFor(sc => sc.PermissionForDataProcessing, f => f.Random.Bool());

            return consents.Generate(count);
        }

        private static void EnrollStudentsInCourses(UniversityContext context, List<Student> students)
        {
            var degreeCourses = context.DegreeCourses.Include(dc => dc.Paths).ToList();

            var random = new Random();

            foreach(var student in students)
            {
                var selectedCourse = degreeCourses[random.Next(degreeCourses.Count)];
                var studentDegreeCourse = new StudentDegreeCourse
                {
                    StudentId = student.Id,
                    DegreeCourseId = selectedCourse.Id
                };
                context.StudentDegreeCourses.Add(studentDegreeCourse);
                student.StudentDegreeCourses.Add(studentDegreeCourse);
            }
        }

        private static void EnrollStudentsInPaths(UniversityContext context, List<Student> students)
        {
            var random = new Random();

            foreach(var student in students)
            {
                var studentDegreeCourse = student.StudentDegreeCourses.FirstOrDefault();
                if(studentDegreeCourse is not null)
                {
                    var degreePaths = context.DegreePaths.Where(dp => dp.DegreeCourseId == studentDegreeCourse.DegreeCourseId).ToList();
                    if (degreePaths.Any())
                    {
                        var selectedPath = degreePaths[random.Next(degreePaths.Count)];
                        var studentDegreePath = new StudentDegreePath
                        {
                            StudentId = student.Id,
                            DegreePathId = selectedPath.Id
                        };
                        context.StudentDegreePaths.Add(studentDegreePath);
                        student.studentDegreePaths.Add(studentDegreePath);
                    }
                }
            }
        }

        private static void EnrollStudentsInModlues(UniversityContext context, List<Student> students)
        {
            var random = new Random();

            foreach(var student in students)
            {
                var studentDegreePath = student.studentDegreePaths.FirstOrDefault();
                if(studentDegreePath is not null)
                {
                    var modules = context.Modules.Where(m => m.DegreePathId == studentDegreePath.DegreePathId).ToList();
                    if (modules.Any())
                    {
                        var selectedModule = modules[random.Next(modules.Count)];
                        var studentModule = new StudentModule
                        {
                            StudentId = student.Id,
                            ModuleId = selectedModule.Id,
                        };
                        context.StudentModules.Add(studentModule);
                        student.StudentModules.Add(studentModule);
                    }
                }
            }
        }
    }
}
