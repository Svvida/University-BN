using Domain.Entities.EducationEntities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.StudentEntities
{
    public class StudentDegreeCourse
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public int DegreeCourseId { get; set; }

        public Student? Student { get; set; }
        public DegreeCourse? DegreeCourse { get; set; }

        public StudentDegreeCourse() { }

        public StudentDegreeCourse(Guid studentId, int degreeCourseId)
        {
            StudentId = studentId;
            DegreeCourseId = degreeCourseId;
        }
    }
}
