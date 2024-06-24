using Domain.EntitiesBase;
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

        public async Task<IEnumerable<T>> GetByFieldAsync(string field, string value)
        {
            var educations = await _educations.Where(e => EF.Property<string>(e, field)==value).ToListAsync();
            if (educations.Any())
            {
                return educations;
            }
            else
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} found with {field} = {value}");
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
    }
}
