using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class ConsentBase
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public bool PermissionForPhoto { get; set; } = false;
        [Required]
        public bool PermissionForDataProcessing { get; set; } = false;

        protected ConsentBase()
        {
        }

        public ConsentBase(Guid id, bool permissionForPhoto, bool permissionForDataProcessing)
        {
            Id = id;
            PermissionForPhoto = permissionForPhoto;
            PermissionForDataProcessing = permissionForDataProcessing;
        }
    }
}
