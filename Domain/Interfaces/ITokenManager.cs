using Domain.Entities.AccountEntities;
using Domain.InMemoryClasses;


namespace Domain.Interfaces
{
    public interface ITokenManager
    {
        string GenerateAccessToken(UserAccount userAccount);
        string GenerateRefreshToken();
        string GenerateSessionId();
        void StoreSession(string userId, string sessionId, string refreshToken, bool rememberMe);
        bool ValidateSession(string sessionId, out string refreshToken);
        void InvalidateSession(string sessionId);
        Task<string> RefreshAccessTokenAsync(string sessionId);
        SessionData GetSession(string sessionId);
        void UpdateLastActivity(string sessionId);
    }
}
