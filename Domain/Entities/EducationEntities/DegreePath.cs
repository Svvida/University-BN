using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EducationEntities
{
    public class DegreePath : EducationBase
    {
        [Required]
        public Guid DegreeCourseId { get; set; }
        public DegreeCourse? DegreeCourse { get; set; }
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<StudentDegreePath> StudentDegreePaths { get; set; } = new List<StudentDegreePath>();

        public DegreePath() { }

        public DegreePath(Guid id, string name, Guid degreeCourseId) : base(id, name)
        {
            DegreeCourseId = degreeCourseId;
        }
    }
}
