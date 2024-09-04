using Domain.Entities.AccountEntities;

namespace Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(Guid id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task CreateAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Guid id);
        Task<Role> GetRoleByAccountId(Guid accountId);
    }
}
