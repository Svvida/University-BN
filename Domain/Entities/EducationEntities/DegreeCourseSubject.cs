using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EducationEntities
{
    public class DegreeCourseSubject
    {
        [Required]
        public Guid DegreeCourseId { get; set; }
        public DegreeCourse? DegreeCourse { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public DegreeCourseSubject() { }

        public DegreeCourseSubject(Guid degreeCourseId, Guid subjectId)
        {
            DegreeCourseId = degreeCourseId;
            SubjectId = subjectId;
        }
    }
}
