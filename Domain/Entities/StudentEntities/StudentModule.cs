using Domain.Entities.EducationEntities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.StudentEntities
{
    public class StudentModule
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public Guid ModuleId { get; set; }

        public Student? Student { get; set; }
        public Module? Module { get; set; }

        public StudentModule() { }

        public StudentModule(Guid studentId, Guid moduleId)
        {
            StudentId = studentId;
            ModuleId = moduleId;
        }
    }
}
