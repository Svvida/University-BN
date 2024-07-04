using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EducationEntities
{
    public class Module : EducationBase
    {
        [Required]
        public int DegreePathId { get; set; }
        public DegreePath? Path { get; set; }
        public ICollection<ModuleSubject> ModuleSubjects { get; set; } = new List<ModuleSubject>();
        public ICollection<StudentModule> StudentModules { get; set; } = new List<StudentModule>();

        public Module() { }

        public Module(int id, string name, int degreePathId) : base(id, name)
        {
            DegreePathId = degreePathId;
        }
    }
}
