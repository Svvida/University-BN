using Application.DTOs.Account.Dtos;
using Application.Interfaces.IGenericServices;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IAccountService : ICRUDService<AccountOnlyDto, AccountFullDto, AccountCreateDto, AccountUpdateDto>
    {
        Task<IEnumerable<AccountOnlyDto>> GetByFieldAsync(AccountSearchableFields field, string value);
    }
}
