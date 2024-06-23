using Domain.EntitiesBase;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Course : EducationBase
    {
        public ICollection<DegreeProgramCourse> ProgramCourses { get; set; } = new List<DegreeProgramCourse>();
        public ICollection<ModuleCourse> ModuleCourses { get; set; } = new List<ModuleCourse>();

        public Course() { }

        public Course(Guid id, string name) : base(id, name) { }
    }
}
