using Domain.Entities.AccountEntities;
using Domain.EntitiesBase;
using Domain.Enums;

namespace Domain.Entities.StudentEntities
{
    public class Student : PersonBase
    {
        public StudentAddress? Address { get; set; }
        public StudentConsent? Consent { get; set; }
        public UserAccount? Account { get; set; }

        // Navigation properies related to enrollment
        public ICollection<StudentDegreeCourse> StudentDegreeCourses { get; set; } = new List<StudentDegreeCourse>();
        public ICollection<StudentDegreePath> studentDegreePaths { get; set; } = new List<StudentDegreePath>();
        public ICollection<StudentModule> StudentModules { get; set; } = new List<StudentModule>();

        public Student()
        {
        }
        public Student(Guid id, string name, string surname, DateTime dateOfBirth, Gender gender, string contactEmail, string contactPhone, DateTime dateOfAddmission, Guid? addressId, Guid? accountId, Guid? consentId)
            : base(id, name, surname, dateOfBirth, gender, contactEmail, contactPhone, dateOfAddmission, addressId, accountId, consentId)
        {
        }
    }
}
