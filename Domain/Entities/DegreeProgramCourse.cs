using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DegreeProgramCourse
    {
        [Required]
        public Guid ProgramId { get; set; }
        public DegreeProgram Program { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public DegreeProgramCourse() { }

        public DegreeProgramCourse(Guid programId, Guid courseId)
        {
            ProgramId = programId;
            CourseId = courseId;
        }
    }
}
