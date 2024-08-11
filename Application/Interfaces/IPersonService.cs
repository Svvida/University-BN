using Domain.Enums.SearchableFields;

namespace Application.Interfaces
{
    public interface IPersonService<T>
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(AccountSearchableFields field, string value);
        Task CreateAsync(T person);
        Task UpdateAsync(T person);
        Task DeleteAsync(Guid id);
    }
}
