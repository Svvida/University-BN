using Domain.EntitiesBase;
using Domain.Enums.SearchableFields;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IPersonRepository<T> where T : PersonBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(PersonSearchableFields field, string value);
        Task CreateAsync(T person);
        Task UpdateAsync(T person);
        Task DeleteAsync(Guid id);
        Task<T> GetByAccountIdAsync(Guid accountId);
    }
}
