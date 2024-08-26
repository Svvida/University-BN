using Domain.Entities.AccountEntities;

namespace Domain.Interfaces
{
    public interface IPasswordResetService
    {
        public Task<UserAccount> ResetUserPassword(string email, string password);
        public Task<string> GeneratePasswordResetTokenAsync(string email);
        public Task<bool> CheckLastPasswordAsync(string email, string newPassword);
    }
}
