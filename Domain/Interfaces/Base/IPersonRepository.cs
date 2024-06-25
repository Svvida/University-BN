using Domain.EntitiesBase;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IPersonRepository<T> where T : PersonBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(string field, string value);
        Task CreateAsync(T person);
        Task UpdateAsync(T person);
        Task DeleteAsync(Guid id);
    }
}
