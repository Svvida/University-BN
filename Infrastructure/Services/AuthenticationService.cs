using Domain.Entities.AccountEntities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;

        public AuthenticationService(IAccountRepository accountRepository, IPasswordHasher<UserAccount> passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
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
    }
}
