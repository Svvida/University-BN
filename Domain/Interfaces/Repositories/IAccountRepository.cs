using Domain.Entities.AccountEntities;

namespace Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<UserAccount> GetByIdAsync(Guid id);
        Task<IEnumerable<UserAccount>> GetAllAsync();
        Task<IEnumerable<UserAccount>> GetByFieldAsync(string field, string value);
        Task CreateAsync(UserAccount userAccount);
        Task UpdateAsync(UserAccount userAccount);
        Task DeleteAsync(Guid id);
    }
}
