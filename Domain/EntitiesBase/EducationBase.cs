using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class EducationBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        protected EducationBase() { }

        protected EducationBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // Property to return the specyfic type name
        public virtual string EntityTypeName => GetType().Name;
    }
}
