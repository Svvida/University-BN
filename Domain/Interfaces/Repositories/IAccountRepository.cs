using Domain.Entities.AccountEntities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<UserAccount> GetByIdAsync(Guid id);
        Task<IEnumerable<UserAccount>> GetAllAsync();
        Task<IEnumerable<UserAccount>> GetByFieldAsync(AccountSearchableFields field, string value);
        Task CreateAsync(UserAccount userAccount);
        Task UpdateAsync(UserAccount userAccount);
        Task DeleteAsync(Guid id);
        Task<UserAccount> GetByUsername(string username);

        IEnumerable<string> GetAllUsernames();
    }
}
