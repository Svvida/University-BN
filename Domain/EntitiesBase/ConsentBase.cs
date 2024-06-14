namespace Domain.EntitiesBase
{
    internal class ConsentBase
    {
        public Guid Id { get; set; }
        public bool PermissionForPhoto { get; set; } = false;
        public bool PermissionForDataProcessing { get; set; } = false;
    }
}
