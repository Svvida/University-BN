using Domain.Entities.AccountEntities;
using Domain.Enums.SearchableFields;

namespace Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<UserAccount> GetByIdAsync(Guid id);
        Task<IEnumerable<UserAccount>> GetAllAsync();
        Task<UserAccount> GetByFieldAsync(AccountSearchableFields field, string value);
        Task CreateAsync(UserAccount userAccount);
        Task UpdateAsync(UserAccount userAccount);
        Task DeleteAsync(Guid id);
        Task<UserAccount> GetByUsername(string username);
        Task<UserAccount?> GetByRefreshTokenAsync(string refreshToken);
        Task<UserAccount?> GetBySessionIdAsync(Guid sessionId);

        IEnumerable<string> GetAllUsernames();
    }
}
