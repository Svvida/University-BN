using Domain.EntitiesBase;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IAddressRepository<T> where T : AddressBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(string field, string value);
        Task CreateAsync(T address);
        Task UpdateAsync(T address);
        Task DeleteAsync(Guid id);
    }
}
