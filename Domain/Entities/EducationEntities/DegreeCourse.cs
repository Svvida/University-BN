using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;

namespace Domain.Entities.EducationEntities
{
    public class DegreeCourse : EducationBase
    {
        public ICollection<DegreePath> Paths { get; set; } = new List<DegreePath>();
        public ICollection<DegreeCourseSubject> DegreeCourseSubjects { get; set; } = new List<DegreeCourseSubject>();
        public ICollection<StudentDegreeCourse> StudentDegreeCourses { get; set; } = new List<StudentDegreeCourse>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public DegreeCourse() { }

        public DegreeCourse(int id, string name) : base(id, name) { }
    }
}
