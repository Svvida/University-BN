using Application.DTOs.BaseDtos;
using Domain.Enums.SearchableFields;

namespace Application.Interfaces
{
    public interface IAddressService<T> where T : AddressOnlyDto
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(AddressSearchableFields field, string value);
        Task CreateAsync(T address);
        Task UpdateAsync(T address);
        Task DeleteAsync(Guid id);
    }
}
