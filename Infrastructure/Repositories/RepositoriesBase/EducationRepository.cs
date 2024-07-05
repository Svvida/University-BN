using Domain.Entities.EducationEntities;
using Domain.EntitiesBase;
using Domain.Enums;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class EducationRepository<T> : IEducationRepository<T> where T : EducationBase
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _educations;

        public EducationRepository(UniversityContext context)
        {
            _context = context;
            _educations = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var education = await _educations.FindAsync(id);
            if(education is not null)
            {
                return education;
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} not found");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _educations.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(EducationSearchableFields field, string value)
        {
            string dbFieldName = GetDbFieldName(field);

            var education = await _educations.Where(e => EF.Property<string>(e, dbFieldName) == value).ToListAsync();

            if (!education.Any())
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} with {field} == {value} was found");
            }

            return education;
        }

        private string GetDbFieldName(EducationSearchableFields field)
        {
            switch (field)
            {
                case EducationSearchableFields.Name:
                    return "Name";
                default:
                    throw new ArgumentException("Invalid field", nameof(field));
            }
        }

        public async Task CreateAsync(T education)
        {
            if (education is null)
            {
                throw new ArgumentNullException(nameof(education), $"{typeof(T).Name} cannot be null");
            }

            await _educations.AddAsync(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T education)
        {
            if (education is null)
            {
                throw new ArgumentNullException(nameof(education), $"{typeof(T).Name} cannot be null");
            }

            _educations.Update(education);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var education = await _educations.FindAsync(id);
            if(education is not null)
            {
                _educations.Remove(education);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with ID: {id} not found");
            }
        }

        public async Task<T> FindAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _educations.AsNoTracking().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<T>> FindAllAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _educations.AsNoTracking().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _educations.AsNoTracking().Any(predicate));
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if(entities is null || !entities.Any())
            {
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null or empty");
            }

            await _educations.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }

    public class ModuleSubjectRepository : IModuleSubjectRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<ModuleSubject> _moduleSubjects;

        public ModuleSubjectRepository(UniversityContext context)
        {
            _context = context;
            _moduleSubjects = context.Set<ModuleSubject>();
        }

        public async Task<ModuleSubject> GetByIdAsync(int moduleId, int subjectId)
        {
            return await _moduleSubjects.FindAsync(moduleId, subjectId);
        }

        public async Task<IEnumerable<ModuleSubject>> GetAllAsync()
        {
            return await _moduleSubjects.ToListAsync();
        }

        public async Task CreateAsync(ModuleSubject moduleSubject)
        {
            await _moduleSubjects.AddAsync(moduleSubject);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<ModuleSubject> moduleSubjects)
        {
            await _moduleSubjects.AddRangeAsync(moduleSubjects);
            await _context.SaveChangesAsync();
        }

        public async Task<ModuleSubject> FindAsync(Func<ModuleSubject, bool> predicate)
        {
            return await Task.Run(() => _moduleSubjects.AsNoTracking().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<ModuleSubject>> FindAllAsync(Func<ModuleSubject, bool> predicate)
        {
            return await Task.Run(() => _moduleSubjects.AsNoTracking().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(Func<ModuleSubject, bool> predicate)
        {
            return await Task.Run(() => _moduleSubjects.AsNoTracking().Any(predicate));
        }
    }

    public class DegreeCourseSubjectRepository : IDegreeCourseSubjectsRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<DegreeCourseSubject> _degreeCourseSubjects;

        public DegreeCourseSubjectRepository(UniversityContext context)
        {
            _context = context;
            _degreeCourseSubjects = context.Set<DegreeCourseSubject>();
        }

        public async Task<DegreeCourseSubject> GetByIdAsync(int degreeCourseId, int subjectId)
        {
            return await _degreeCourseSubjects.FindAsync(degreeCourseId, subjectId);
        }

        public async Task<IEnumerable<DegreeCourseSubject>> GetAllAsync()
        {
            return await _degreeCourseSubjects.ToListAsync();
        }

        public async Task CreateAsync(DegreeCourseSubject degreeCourseSubject)
        {
            await _degreeCourseSubjects.AddAsync(degreeCourseSubject);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<DegreeCourseSubject> degreeCourseSubjects)
        {
            await _degreeCourseSubjects.AddRangeAsync(degreeCourseSubjects);
            await _context.SaveChangesAsync();
        }

        public async Task<DegreeCourseSubject> FindAsync(Func<DegreeCourseSubject, bool> predicate)
        {
            return await Task.Run(() => _degreeCourseSubjects.AsNoTracking().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<DegreeCourseSubject>> FindAllAsync(Func<DegreeCourseSubject, bool> predicate)
        {
            return await Task.Run(() => _degreeCourseSubjects.AsNoTracking().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(Func<DegreeCourseSubject, bool> predicate)
        {
            return await Task.Run(() => _degreeCourseSubjects.AsNoTracking().Any(predicate));
        }
    }
}
