using Domain.Entities.EducationEntities;
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

    public interface IModuleSubjectRepository
    {
        Task<ModuleSubject> GetByIdAsync(int moduleId, int subjectId);
        Task<IEnumerable<ModuleSubject>> GetAllAsync();
        Task CreateAsync(ModuleSubject moduleSubject);
        Task AddRangeAsync(IEnumerable<ModuleSubject> moduleSubjects);
        // Predicate-based methods
        Task<ModuleSubject> FindAsync(Func<ModuleSubject, bool> predicate);
        Task<IEnumerable<ModuleSubject>> FindAllAsync(Func<ModuleSubject, bool> predicate);
        Task<bool> ExistsAsync(Func<ModuleSubject, bool> predicate);
    }

    public interface IDegreeCourseSubjectsRepository
    {
        Task<DegreeCourseSubject> GetByIdAsync(int degreeCourseId, int subjectId);
        Task<IEnumerable<DegreeCourseSubject>> GetAllAsync();
        Task CreateAsync(DegreeCourseSubject degreeCourseSubject);
        Task AddRangeAsync(IEnumerable<DegreeCourseSubject> degreeCourseSubjects);
        // Predicate-based methods
        Task<DegreeCourseSubject> FindAsync(Func<DegreeCourseSubject, bool> predicate);
        Task<IEnumerable<DegreeCourseSubject>> FindAllAsync(Func<DegreeCourseSubject, bool> predicate);
        Task<bool> ExistsAsync(Func<DegreeCourseSubject, bool> predicate);
    }
}
