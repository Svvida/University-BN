using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class ConsentBase
    {
        [Key]
        public Guid Id { get; set; }

        public bool PermissionForPhoto { get; set; }
        public bool PermissionForDataProcessing { get; set; }

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
