using Domain.EntitiesBase;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class AddressRepository<T> : IAddressRepository<T> where T : AddressBase
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _addresses;

        public AddressRepository(UniversityContext context)
        {
            _context = context;
            _addresses = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var address = await _addresses.FindAsync(id);
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
            return await _addresses.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(string field, string value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException("Field name cannot be empty", nameof(field));
            }
            var address = await _addresses.Where(e => EF.Property<string>(e, field) == value).ToListAsync();
            if (address is not null)
            {
                return address;
            }
            else
            {
                throw new KeyNotFoundException("No addresses found with the specified field and value");
            }
        }

        public async Task CreateAsync(T address)
        {
            await _addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T address)
        {
            _addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _addresses.FindAsync(id);
            if (address is not null)
            {
                _addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Address not found");
            }
        }
    }
}
