using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EducationEntities
{
    public class ModuleSubject
    {
        [Required]
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public ModuleSubject() { }

        public ModuleSubject(int moduleId, int subjectId)
        {
            ModuleId = moduleId;
            SubjectId = subjectId;
        }
    }
}
