using Domain.Entities.EducationEntities;
using Domain.Interfaces.InterfacesBase;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IEducationRepository<DegreeCourse> DegreeCourses { get; }
        IEducationRepository<DegreePath> DegreePaths { get; }
        IEducationRepository<Module> Modules { get; }
        IEducationRepository<Subject> Subjects { get; }
        IModuleSubjectRepository ModuleSubjects { get; }
        IDegreeCourseSubjectsRepository DegreeCourseSubjects { get; }
        Task<int> CompleteAsync();
    }
}
