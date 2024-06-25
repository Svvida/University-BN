using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EducationEntities
{
    public class ModuleSubject
    {
        [Required]
        public Guid ModuleId { get; set; }
        public Module? Module { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public ModuleSubject() { }

        public ModuleSubject(Guid moduleId, Guid subjectId)
        {
            ModuleId = moduleId;
            SubjectId = subjectId;
        }
    }
}
