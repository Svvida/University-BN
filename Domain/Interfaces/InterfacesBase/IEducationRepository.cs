using Domain.EntitiesBase;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IEducationRepository<T> where T : EducationBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
