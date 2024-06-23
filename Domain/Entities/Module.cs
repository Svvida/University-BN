using Domain.EntitiesBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Module : EducationBase
    {
        [Required]
        public Guid DegreePathId { get; set; }
        public DegreePath Path { get; set; }
        public ICollection<ModuleCourse> ModuleCourses { get; set; } = new List<ModuleCourse>();

        public Module() { }

        public Module(Guid id, string name, Guid degreePathId) : base(id, name)
        {
            DegreePathId = degreePathId;
        }
    }
}
