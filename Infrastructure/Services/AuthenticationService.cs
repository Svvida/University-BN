using Domain.Entities.AccountEntities;
using Domain.Enums.SearchableFields;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public AuthenticationService(
            IAccountRepository accountRepository,
            IPasswordHasher<UserAccount> passwordHasher,
            ILogger<AuthenticationService> logger,
            IRefreshTokenStore refreshTokenStore)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var account = await _accountRepository.GetByFieldAsync(AccountSearchableFields.Login, username);
            if (account is null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(account, account.Password, password);

            return result == PasswordVerificationResult.Success;
        }

        public bool ValidateRefreshToken(string userId, string sessionId, string refreshToken)
        {
            var isValid = _refreshTokenStore.ValidateToken(userId, sessionId, refreshToken);
            if (!isValid)
            {
                _logger.LogInformation("Invalid or expired refresh token");
            }

            return isValid;
        }

        public bool ValidateSession(string userId, string sessionId)
        {
            var isValid = _refreshTokenStore.ValidateToken(userId, sessionId, null);
            if (isValid)
            {
                _logger.LogInformation($"Valid session ID: {sessionId} for user: {userId}");
            }
            else
            {
                _logger.LogInformation("Invalid session ID or session does not belong to the user.");
            }

            return isValid;
        }

        public void StoreRefreshToken(string userId, string sessionId, string refreshToken)
        {
            var expiryTime = DateTime.UtcNow.AddDays(7);
            _refreshTokenStore.StoreToken(userId, sessionId, refreshToken, expiryTime);
        }
    }
}
