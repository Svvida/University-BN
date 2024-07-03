using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.EducationEntities
{
    public class DegreeCourseSubject
    {
        [Required]
        public int DegreeCourseId { get; set; }
        public DegreeCourse? DegreeCourse { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public DegreeCourseSubject() { }

        public DegreeCourseSubject(int degreeCourseId, int subjectId)
        {
            DegreeCourseId = degreeCourseId;
            SubjectId = subjectId;
        }
    }
}
