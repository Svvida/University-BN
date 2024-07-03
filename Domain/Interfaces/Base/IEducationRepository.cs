using Domain.EntitiesBase;
using Domain.Enums;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IEducationRepository<T> where T : EducationBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(EducationSearchableFields field, string value);
        Task CreateAsync(T education);
        Task UpdateAsync(T education);
        Task DeleteAsync(Guid id);
        
        // Predicate-based methods
        Task<T> FindAsync(Func<T, bool> predicate);
        Task<IEnumerable<T>> FindAllAsync(Func<T, bool> predicate);
        Task<bool> ExistsAsync(Func<T, bool> predicate);

        // AddRangeAsync method
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
