namespace Domain.InMemoryClasses
{
    public class SessionData
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }
        public DateTime LastActivity { get; set; }
        public bool RememberMe { get; set; }
    }
}
