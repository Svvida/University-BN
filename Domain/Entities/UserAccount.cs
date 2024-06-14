namespace Domain.Entities
{
    internal class UserAccount
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = "Default";
        public string Password { get; set; } = "Default";
        public string Email { get; set; } = "Default";
        public ICollection<Role>? Roles { get; set; }
    }
}
