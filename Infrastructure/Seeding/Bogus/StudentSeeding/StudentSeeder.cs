using Bogus;
using Bogus.Extensions.Poland;
using Domain.Entities.EducationEntities;
using Domain.Entities.StudentEntities;
using Domain.Enums;
using Infrastructure.Seeding.Bogus.AccountSeeding;

namespace Infrastructure.Seeding.Bogus.StudentSeeding
{
    public class StudentSeeder
    {
        private readonly AccountSeeder _accountSeeder;

        public StudentSeeder(AccountSeeder accountSeeder)
        {
            _accountSeeder = accountSeeder;
        }

        public List<Student> GenerateStudents(int count)
        {
            var students = new List<Student>();

            for (int i = 0; i < count; i++)
            {
                var student = new Faker<Student>()
                    .RuleFor(s => s.Id, f => Guid.NewGuid())
                    .RuleFor(s => s.Name, f => f.Name.FirstName())
                    .RuleFor(s => s.Surname, f => f.Name.LastName())
                    .RuleFor(s => s.DateOfBirth, f => f.Date.Past(20, DateTime.Now.AddYears(-18)))
                    .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                    .RuleFor(s => s.PESEL, f => f.Person.Pesel())
                    .RuleFor(s => s.ContactEmail, f => f.Internet.Email())
                    .RuleFor(s => s.ContactPhone, f => f.Phone.PhoneNumberFormat())
                    .RuleFor(s => s.DateOfAddmission, f => f.Date.Past(3))
                    .Generate();

                // Generate and associate account with student
                student.Account = _accountSeeder.GenerateAccountForPerson(student.Name, student.Surname, RoleType.Student.ToString());

                // Add additional related entities
                student.Address = GenerateAddress();
                student.Consent = GenerateConsent();

                students.Add(student);
            }

            return students;
        }

        private StudentAddress GenerateAddress()
        {
            return new Faker<StudentAddress>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Country, f => f.Address.Country())
                .RuleFor(s => s.City, f => f.Address.City())
                .RuleFor(s => s.PostalCode, f => f.Address.ZipCode())
                .RuleFor(s => s.Street, f => f.Address.StreetAddress())
                .RuleFor(s => s.BuildingNumber, f => f.Address.BuildingNumber())
                .RuleFor(s => s.ApartmentNumber, f => f.Random.Bool() ? f.Address.BuildingNumber() : null)
                .Generate();
        }

        private StudentConsent GenerateConsent()
        {
            return new Faker<StudentConsent>()
                .RuleFor(sc => sc.Id, f => Guid.NewGuid())
                .RuleFor(sc => sc.PermissionForPhoto, f => f.Random.Bool())
                .RuleFor(sc => sc.PermissionForDataProcessing, f => f.Random.Bool())
                .Generate();
        }

        public void EnrollStudentsInCourses(List<Student> students, List<DegreeCourse> courses)
        {
            var random = new Random();

            foreach (var student in students)
            {
                var selectedCourse = courses[random.Next(courses.Count)];
                var studentDegreeCourse = new StudentDegreeCourse
                {
                    StudentId = student.Id,
                    DegreeCourseId = selectedCourse.Id
                };
                student.StudentDegreeCourses.Add(studentDegreeCourse);
            }
        }

        public void EnrollStudentsInPaths(List<Student> students, List<DegreeCourse> courses)
        {
            var random = new Random();

            foreach (var student in students)
            {
                var studentDegreeCourse = student.StudentDegreeCourses.FirstOrDefault();
                if (studentDegreeCourse is not null)
                {
                    var degreePaths = courses.FirstOrDefault(dc => dc.Id == studentDegreeCourse.DegreeCourseId)?.Paths;


                    if (degreePaths is not null && degreePaths.Any())
                    {
                        var selectedPath = degreePaths.ElementAt(random.Next(degreePaths.Count));
                        var studentDegreePath = new StudentDegreePath
                        {
                            StudentId = student.Id,
                            DegreePathId = selectedPath.Id
                        };
                        student.studentDegreePaths.Add(studentDegreePath);
                    }
                }
            }
        }

        public void EnrollStudentsInModlues(List<Student> students, List<DegreeCourse> courses)
        {
            var random = new Random();

            foreach (var student in students)
            {
                var studentDegreePath = student.studentDegreePaths.FirstOrDefault();
                if (studentDegreePath is not null)
                {
                    var modules = courses.SelectMany(dc => dc.Paths)
                        .Where(dp => dp.Id == studentDegreePath.DegreePathId)
                        .SelectMany(dp => dp.Modules)
                        .ToList();

                    if (modules.Any())
                    {
                        var selectedModule = modules[random.Next(modules.Count)];
                        var studentModule = new StudentModule
                        {
                            StudentId = student.Id,
                            ModuleId = selectedModule.Id,
                        };
                        student.StudentModules.Add(studentModule);
                    }
                }
            }
        }
    }
}
