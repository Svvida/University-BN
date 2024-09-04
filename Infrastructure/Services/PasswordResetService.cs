using Domain.Entities.AccountEntities;
using Domain.Enums.SearchableFields;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordHasher<UserAccount> _passwordHasher;
        private readonly IAccountRepository _accountRepository;
        private readonly UniversityContext _context;
        private readonly ILogger<PasswordResetService> _logger;

        public PasswordResetService(
            IPasswordHasher<UserAccount> passwordHasher,
            IAccountRepository accountRepository,
            UniversityContext context,
            ILogger<PasswordResetService> logger)
        {
            _passwordHasher = passwordHasher;
            _accountRepository = accountRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<UserAccount> ResetUserPassword(string email, string newPassword)
        {
            // Retrieve account associated with provided email
            var account = await _context.UsersAccounts.FirstOrDefaultAsync(ua => ua.Email == email);
            if (account is null)
            {
                throw new KeyNotFoundException($"Account with email {email} not found");
            }

            account.Password = _passwordHasher.HashPassword(account, newPassword);

            // Update account with new password and clear reset token
            await _accountRepository.UpdateAsync(account);
            await _context.SaveChangesAsync(CancellationToken.None);
            _logger.LogInformation("Password successfully changed");

            return account;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _accountRepository.GetByFieldAsync(AccountSearchableFields.Email, email);

            if (user is null)
            {
                throw new KeyNotFoundException($"No user found with email {email}");
            }

            var resetToken = Guid.NewGuid().ToString();

            return resetToken;
        }

        public async Task<bool> CheckLastPasswordAsync(string email, string newPassword)
        {
            var user = await _accountRepository.GetByFieldAsync(AccountSearchableFields.Email, email);

            if (user is null)
            {
                throw new KeyNotFoundException($"No user found with email {email}");
            }

            return _passwordHasher.VerifyHashedPassword(user, user.Password, newPassword) == PasswordVerificationResult.Success;
        }
    }
}
