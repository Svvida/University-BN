using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class EducationBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        protected EducationBase() { }

        protected EducationBase(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
