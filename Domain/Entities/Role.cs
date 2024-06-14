namespace Domain.Entities
{
    internal class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Default";
        public ICollection<UserAccount>? Accounts { get; set; }
    }
}
