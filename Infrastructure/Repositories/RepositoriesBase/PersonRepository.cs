using Domain.EntitiesBase;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class PersonRepository<T> : IPersonRepository<T> where T : PersonBase
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _persons;

        public PersonRepository(UniversityContext context)
        {
            _context = context;
            _persons = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var person = await _persons.FindAsync(id);

            if(person is not null)
            {
                return person;
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with ID: {id} was not found");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _persons.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(string field, string value)
        {
            var persons = await _persons.Where(e => EF.Property<string>(e, field) == value).ToListAsync();
            if (persons.Any())
            {
                return persons;
            }
            else
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} found with {field} = {value}");
            }
        }

        public async Task CreateAsync(T person)
        {
            if(person is null)
            {
                throw new ArgumentNullException(nameof(person), $"{typeof(T).Name} cannot be null");
            }
            await _persons.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T person)
        {
            if(person is null)
            {
                throw new ArgumentNullException(nameof(person), $"{typeof(T).Name} cannot be null");
            }
            _persons.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var person = await _persons.FindAsync(id);
            if(person is not null)
            {
                _persons.Remove(person);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} found with ID: {id}");
            }
        }
    }
}
