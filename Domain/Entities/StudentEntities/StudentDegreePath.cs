using Domain.Entities.EducationEntities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.StudentEntities
{
    public class StudentDegreePath
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public Guid DegreePathId { get; set; }

        public Student? Student { get; set; }
        public DegreePath? DegreePath { get; set; }

        public StudentDegreePath() { }

        public StudentDegreePath(Guid studentId, Guid degreePathId)
        {
            StudentId = studentId;
            DegreePathId = degreePathId;
        }
    }
}
