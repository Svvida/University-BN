using Application.DTOs.BaseDtos;
using Application.Interfaces.IGenericServices;
using Domain.Enums.SearchableFields;

namespace Application.Interfaces
{
    public interface IPersonService<T> : ICRUDService<PersonOnlyDto, PersonExtendedDto, PersonCreateDto, PersonExtendedDto>
    {
        Task<IEnumerable<T>> GetByFieldAsync(PersonSearchableFields field, string value);
    }
}
