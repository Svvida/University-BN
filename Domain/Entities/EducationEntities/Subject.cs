using Domain.EntitiesBase;

namespace Domain.Entities.EducationEntities
{
    public class Subject : EducationBase
    {
        public ICollection<DegreeCourseSubject> DegreeCourseSubjects { get; set; } = new List<DegreeCourseSubject>();
        public ICollection<ModuleSubject> ModuleSubjects { get; set; } = new List<ModuleSubject>();

        public Subject() { }

        public Subject(int id, string name) : base(id, name) { }
    }
}
