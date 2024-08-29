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
            var account = await _accountRepository.GetByFieldAsync(AccountSearchableFields.Login, username);
            if (account is null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(account, account.Password, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
