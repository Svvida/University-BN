using Domain.EntitiesBase;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class AddressRepository<T> : IAddressRepository<T> where T : AddressBase
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _dbSet;

        public AddressRepository(UniversityContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var address = await _dbSet.FindAsync(id);
            if (address is not null)
            {
                return address;
            }
            else
            {
                throw new KeyNotFoundException("Address not found");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(string field, string value)
        {
            var address = await _dbSet.Where(e => EF.Property<string>(e, field) == value).ToListAsync();
            if (address is not null)
            {
                return address;
            }
            else
            {
                throw new KeyNotFoundException("Address not found");
            }
        }

        public async Task CreateAsync(T address)
        {
            await _dbSet.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T address)
        {
            _dbSet.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _dbSet.FindAsync(id);
            if (address is not null)
            {
                _dbSet.Remove(address);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Address not found");
            }
        }
    }
}
