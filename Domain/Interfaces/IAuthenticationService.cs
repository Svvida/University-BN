using Domain.Entities.AccountEntities;

namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUserAsync(string username, string password);
        Task<UserAccount> GetUserAsync(string username);
        Task<UserAccount?> ValidateRefreshTokenAsync(string refreshToken);
    }
}
