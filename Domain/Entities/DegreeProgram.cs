using Domain.EntitiesBase;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DegreeProgram : EducationBase
    {
        public ICollection<DegreePath> Paths { get; set; } = new List<DegreePath>();
        public ICollection<DegreeProgramCourse> ProgramCourses { get; set; } = new List<DegreeProgramCourse>();

        public DegreeProgram() { }

        public DegreeProgram(Guid id, string name) : base(id, name) { }
    }
}
