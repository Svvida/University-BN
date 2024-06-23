using Domain.EntitiesBase;

namespace Domain.Interfaces.InterfacesBase
{
    internal interface IConsentRepository<T> where T : ConsentBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(string field, string value);
        Task CreateAsync(T consent);
        Task UpdateAsync(T consent);
        Task DeleteAsync(Guid id);
    }
}
