using Domain.EntitiesBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DegreePath : EducationBase
    {
        [Required]
        public Guid ProgramId { get; set; }
        public DegreeProgram Program { get; set; }
        public ICollection<Module> Modules { get; set; } = new List<Module>();

        public DegreePath() { }

        public DegreePath(Guid id, string name, Guid programId) : base(id, name)
        {
            ProgramId = programId;
        }
    }
}
