using Domain.EntitiesBase;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IEducationRepository<T> where T : EducationBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(string field, string value);
        Task CreateAsync(T education);
        Task UpdateAsync(T education);
        Task DeleteAsync(Guid id);
    }
}
