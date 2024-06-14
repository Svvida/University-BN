namespace Domain.Entities
{
    internal class UserAccountRole
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public UserAccount? Account { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
