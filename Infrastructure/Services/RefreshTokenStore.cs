using Domain.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly ConcurrentDictionary<string, (string refreshToken, DateTime expiry)> _tokens
            = new ConcurrentDictionary<string, (string refreshToken, DateTime expiry)>();

        public void StoreToken(string userId, string sessionId, string refreshToken, DateTime expiry)
        {
            var key = GenerateToken(userId, sessionId);
            _tokens[key] = (refreshToken, expiry);
        }

        public bool ValidateToken(string userId, string sessionId, string refreshToken)
        {
            var key = GenerateToken(userId, sessionId);

            if(_tokens.TryGetValue(key, out var tokenData))
            {
                if(tokenData.refreshToken == refreshToken && tokenData.expiry > DateTime.UtcNow)
                {
                    return true;
                }

                // Invalidate token if its expired
                _tokens.TryRemove(key, out tokenData);
            }
            return false;
        }

        public void InvalidateToken(string userId, string sessionId)
        {
            var key = GenerateToken(userId, sessionId);
            _tokens.TryRemove(key, out _);
        }

        private string GenerateToken(string userId, string sessionId)
        {
            return $"{userId}_{sessionId}";
        }
    }
}
