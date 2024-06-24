using Domain.EntitiesBase;

namespace Domain.Interfaces.InterfacesBase
{
    public interface IConsentRepository<T> where T : ConsentBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T consent);
        Task UpdateAsync(T consent);
        Task DeleteAsync(Guid id);
        Task SwitchConsentAsync(Guid id, string consentType);
    }
}
