using Domain.Entities.AccountEntities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly ConcurrentDictionary<string, (string refreshToken, DateTime expiry)> _sessions
            = new ConcurrentDictionary<string, (string refreshToken, DateTime expiry)>();

        private readonly IJwtService _jwtService;
        private readonly ILogger<TokenManager> _logger;
        private readonly IAccountRepository _accountRepository;

        public TokenManager(IJwtService jwtService,
            ILogger<TokenManager> logger,
            IAccountRepository accountRepository)
        {
            _jwtService = jwtService;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        private string GenerateCompositeKey(string userId, string sessionId)
        {
            return $"{userId}_{sessionId}";
        }

        public string GenerateAccessToken(UserAccount userAccount)
        {
            var token = _jwtService.GenerateToken(userAccount);
            _logger.LogInformation($"Generated access token for user: {userAccount.Login}.");
            return token;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public void StoreSession(string userId, string sessionId, string refreshToken)
        {
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var key = GenerateCompositeKey(userId, sessionId);
            _sessions[key] = (refreshToken, refreshTokenExpiryTime);
            _logger.LogInformation($"Stored session with sessionId: {sessionId} for userId: {userId}, expiry: {refreshTokenExpiryTime}.");
        }

        public bool ValidateSession(string userId, string sessionId, out string refreshToken)
        {
            refreshToken = null;
            var key = GenerateCompositeKey(userId, sessionId);

            if(_sessions.TryGetValue(key, out var sessionData))
            {
                if(sessionData.expiry > DateTime.UtcNow)
                {
                    refreshToken = sessionData.refreshToken;
                    _logger.LogInformation($"Session is valid for userId: {userId}, sessionId: {sessionId}");
                    return true;
                }

                // Invalidate session if expired
                _sessions.TryRemove(key, out _);
                _logger.LogInformation($"Session expired for userId: {userId}, sessionId: {sessionId}");
            }
            _logger.LogInformation($"Session is invalid or expired for userId: {userId}, sessionId: {sessionId}");
            return false;
        }

        public void InvalidateSession(string userId, string sessionId)
        {
            var key = GenerateCompositeKey(userId, sessionId);
            _sessions.TryRemove(key, out _);
            _logger.LogInformation($"Invalidated session for userId: {userId}, sessionId: {sessionId}");
        }

        public async Task<string> RefreshAccessTokenAsync(string userId, string sessionId)
        {
            if(ValidateSession(userId, sessionId, out var refreshToken))
            {
                var userAccount = await _accountRepository.GetByIdAsync(new Guid(userId));
                var newAccessToken = GenerateAccessToken(userAccount);

                return newAccessToken;
            }

            return null;
        }
    }
}
