using Domain.EntitiesBase;
using Domain.Enums;
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

        public async Task<IEnumerable<T>> GetByFieldAsync(AddressSearchableFields field, string value)
        {
            string dbFieldName = GetDbFieldName(field);
            var address = await _addresses.Where(e => EF.Property<string>(e, dbFieldName) == value)
                .ToListAsync();

            if(!address.Any())
            {
                throw new KeyNotFoundException($"No address found with {dbFieldName} = {value}");
            }

            return address;
        }

        private string GetDbFieldName(AddressSearchableFields field)
        {
            switch(field)
            {
                case AddressSearchableFields.City:
                    return "City";
                case AddressSearchableFields.Country:
                    return "Country";
                case AddressSearchableFields.PostalCode:
                    return "PostalCode";
                case AddressSearchableFields.Street:
                    return "Street";
                case AddressSearchableFields.BuildingNumber:
                    return "BuildingNumber";
                case AddressSearchableFields.ApartmentNumber:
                    return "ApartmentNumber";
                default:
                    throw new ArgumentOutOfRangeException(nameof(field), field, "Unknown field");
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
