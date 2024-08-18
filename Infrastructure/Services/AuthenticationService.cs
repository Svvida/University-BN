using Domain.Entities.AccountEntities;
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

        public AuthenticationService(
            IAccountRepository accountRepository,
            IPasswordHasher<UserAccount> passwordHasher,
            ILogger<AuthenticationService> logger)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var account = await _accountRepository.GetByUsername(username);
            if (account is null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(account, account.Password, password);

            return result == PasswordVerificationResult.Success;
        }

        public async Task<UserAccount> GetUserAsync(string username)
        {
            return await _accountRepository.GetByUsername(username);
        }

        public async Task<UserAccount?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var account = await _accountRepository.GetByRefreshTokenAsync(refreshToken);
            if (account is null || account.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                _logger.LogInformation("Invalid or expired refresh token");
                return null;
            }

            return account;
        }

        public async Task<UserAccount?> ValidateSessionAsync(Guid sessionId)
        {
            var account = await _accountRepository.GetBySessionIdAsync(sessionId);
            if (account is null)
            {
                _logger.LogInformation("Invalid session ID");
                return null;
            }

            return account;
        }
    }
}
