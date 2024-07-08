using Application.DTOs.AccountDtos;
using Application.Interfaces.IGenericServices;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IAccountService : ICRUDService<AccountOnlyDto, AccountFullDto, AccountCreateDto, AccountFullDto>
    {
        Task<IEnumerable<AccountOnlyDto>> GetByFieldAsync(AccountSearchableFields field, string value);
    }
}
