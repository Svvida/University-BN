using Domain.Entities.EducationEntities;
using Domain.Interfaces;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Infrastructure.Repositories.RepositoriesBase;

namespace Infrastructure.Seeding
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityContext _context;

        public UnitOfWork(UniversityContext context)
        {
            _context = context;
            DegreeCourses = new EducationRepository<DegreeCourse>(_context);
            DegreePaths = new EducationRepository<DegreePath>(_context);
            Modules = new EducationRepository<Module>(_context);
            Subjects = new EducationRepository<Subject>(_context);
            ModuleSubjects = new ModuleSubjectRepository(_context);
            DegreeCourseSubjects = new DegreeCourseSubjectRepository(_context);
        }

        public IEducationRepository<DegreeCourse> DegreeCourses { get; private set; }
        public IEducationRepository<DegreePath> DegreePaths { get; private set; }
        public IEducationRepository<Module> Modules { get; private set; }
        public IEducationRepository<Subject> Subjects { get; private set; }
        public IDegreeCourseSubjectsRepository DegreeCourseSubjects { get; private set; }
        public IModuleSubjectRepository ModuleSubjects { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
