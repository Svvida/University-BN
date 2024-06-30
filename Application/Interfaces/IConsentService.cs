using Application.DTOs.BaseDtos;

namespace Application.Interfaces
{
    public interface IConsentService<T> where T : ConsentDto
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T consent);
        Task UpdateAsync(T consent);
        Task DeleteAsync(Guid id);
        Task SwitchConsentAsync(Guid id, string consentType);
    }
}
