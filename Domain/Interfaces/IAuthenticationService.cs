namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUserAsync(string username, string password);
        bool ValidateRefreshToken(string userId, string sessionId, string refreshToken);
        bool ValidateSession(string userId, string sessionId);
        void StoreRefreshToken(string userId, string sessionId, string refreshToken);
    }
}
