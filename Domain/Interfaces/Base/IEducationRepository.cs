﻿using Domain.EntitiesBase;
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
    }
}
