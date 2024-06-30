using Application.DTOs.AccountDtos;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> GetByIdAsync(Guid id);
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task CreateAsync(RoleDto role);
        Task UpdateAsync(RoleDto role);
        Task DeleteAsync(Guid id);
    }
}
