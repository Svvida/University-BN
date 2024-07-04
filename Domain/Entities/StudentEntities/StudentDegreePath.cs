using Domain.Entities.EducationEntities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.StudentEntities
{
    public class StudentDegreePath
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public int DegreePathId { get; set; }

        public Student? Student { get; set; }
        public DegreePath? DegreePath { get; set; }

        public StudentDegreePath() { }

        public StudentDegreePath(Guid studentId, int degreePathId)
        {
            StudentId = studentId;
            DegreePathId = degreePathId;
        }
    }
}
