using Application.DTOs.AccountDtos;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountFullDto> GetByIdAsync(Guid id);
        Task<IEnumerable<AccountOnlyDto>> GetAllAsync();
        Task<IEnumerable<AccountOnlyDto>> GetByFieldAsync(AccountSearchableFields field, string value);
        Task CreateAsync(AccountFullDto userAccount);
        Task UpdateAsync(AccountFullDto userAccount);
        Task DeleteAsync(Guid id);
    }
}
