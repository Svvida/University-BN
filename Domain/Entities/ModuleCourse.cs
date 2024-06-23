using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ModuleCourse
    {
        [Required]
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public ModuleCourse() { }

        public ModuleCourse(Guid moduleId, Guid courseId)
        {
            ModuleId = moduleId;
            CourseId = courseId;
        }
    }
}
