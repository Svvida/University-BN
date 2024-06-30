using Application.DTOs.BaseDtos;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IEducationService<T> where T : EducationDto
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByFieldAsync(EducationSearchableFields field, string value);
        Task CreateAsync(T education);
        Task UpdateAsync(T education);
        Task DeleteAsync(Guid id);
    }
}
