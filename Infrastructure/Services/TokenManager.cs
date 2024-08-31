using Domain.Entities.AccountEntities;
using Domain.InMemoryClasses;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly ConcurrentDictionary<string, SessionData> _sessions
            = new ConcurrentDictionary<string, SessionData>();

        private readonly IJwtService _jwtService;
        private readonly ILogger<TokenManager> _logger;
        private readonly IAccountRepository _accountRepository;

        private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(20);

        public TokenManager(IJwtService jwtService,
            ILogger<TokenManager> logger,
            IAccountRepository accountRepository)
        {
            _jwtService = jwtService;
            _logger = logger;
            _accountRepository = accountRepository;
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

        public void StoreSession(string userId, string sessionId, string refreshToken, bool rememberMe)
        {
            var refreshTokenExpiryTime = rememberMe ?
                DateTime.UtcNow.AddDays(7)
                : DateTime.UtcNow.AddMinutes(20);
            var sessionData = new SessionData
            {
                UserId = userId,
                RefreshToken = refreshToken,
                Expiry = refreshTokenExpiryTime,
                LastActivity = DateTime.UtcNow,
                RememberMe = rememberMe
            };
            _sessions[sessionId] = sessionData;
            _logger.LogInformation($"Stored session with sessionId: {sessionId} for userId: {userId}, expiry: {refreshTokenExpiryTime}.");
        }

        public bool ValidateSession(string sessionId, out string refreshToken)
        {
            refreshToken = null;

            if (_sessions.TryGetValue(sessionId, out var sessionData))
            {
                if (sessionData.Expiry > DateTime.UtcNow)
                {
                    // If not "Remember Me", check and possibly update LastActivity
                    if (!sessionData.RememberMe)
                    {
                        if (sessionData.LastActivity.Add(_sessionTimeout) > DateTime.UtcNow)
                        {
                            // Sliding expiration handled by middleware
                            refreshToken = sessionData.RefreshToken;
                            _logger.LogInformation($"Session is valid and active for sessionId: {sessionId}");
                            return true;
                        }
                        else
                        {
                            // Session expired due to inactivity
                            _sessions.TryRemove(sessionId, out _);
                            _logger.LogInformation($"Session expired due to inactivity for sessionId: {sessionId}");
                        }
                    }
                    else
                    {
                        // "Remember Me" sessions do not use sliding expiration
                        refreshToken = sessionData.RefreshToken;
                        _logger.LogInformation($"Session is valid for 'Remember Me' sessionId: {sessionId}");
                        return true;
                    }
                }
                else
                {
                    // Session expired
                    _sessions.TryRemove(sessionId, out _);
                    _logger.LogInformation($"Session expired for sessionId: {sessionId}");
                }
            }

            _logger.LogInformation($"Session is invalid or expired for sessionId: {sessionId}");
            return false;
        }

        public void InvalidateSession(string sessionId)
        {
            _sessions.TryRemove(sessionId, out _);
            _logger.LogInformation($"Invalidated session with sessionId: {sessionId}");
        }

        public async Task<string> RefreshAccessTokenAsync(string sessionId)
        {
            if(ValidateSession(sessionId, out var refreshToken))
            {
                if(_sessions.TryGetValue(sessionId, out var sessionData))
                {
                    var userAccount = await _accountRepository.GetByIdAsync(new Guid(sessionData.UserId));
                    var newAccessToken = GenerateAccessToken(userAccount);

                    return newAccessToken;
                }
            }

            return null;
        }

        public SessionData GetSession(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var sessionData);
            return sessionData;
        }

        public void UpdateLastActivity(string sessionId)
        {
            if( _sessions.TryGetValue(sessionId, out var sessionData))
            {
                sessionData.LastActivity = DateTime.UtcNow;
                _logger.LogInformation($"LastActivity updated for sessionId: {sessionId} to {sessionData.LastActivity}");
            }
        }
    }
}
