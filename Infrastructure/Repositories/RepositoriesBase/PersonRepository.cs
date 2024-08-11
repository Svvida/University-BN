using Domain.EntitiesBase;
using Domain.Enums.SearchableFields;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class PersonRepository<T> : IPersonRepository<T> where T : PersonExtendedBase
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

            if (person is not null)
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

        public async Task<IEnumerable<T>> GetByFieldAsync(PersonSearchableFields field, string value)
        {

            string dbFieldName = GetDbFieldName(field);
            var person = await _persons.Where(e => EF.Property<string>(e, dbFieldName) == value).ToListAsync();

            if (!person.Any())
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} found with {field} = {value}");
            }

            return person;
        }

        private string GetDbFieldName(PersonSearchableFields field)
        {
            switch (field)
            {
                case PersonSearchableFields.Name:
                    return "Name";
                case PersonSearchableFields.Surname:
                    return "Surname";
                case PersonSearchableFields.DateOfBirth:
                    return "DateOfBirth";
                case PersonSearchableFields.ContactEmail:
                    return "ContactEmail";
                case PersonSearchableFields.ContactPhone:
                    return "ContactPhone";
                case PersonSearchableFields.DateOfAddmission:
                    return "DateOfAddmission";
                default:
                    throw new ArgumentException("Invalid field", nameof(field));
            }
        }

        public async Task CreateAsync(T person)
        {
            if (person is null)
            {
                throw new ArgumentNullException(nameof(person), $"{typeof(T).Name} cannot be null");
            }
            await _persons.AddAsync(person);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task UpdateAsync(T person)
        {
            if (person is null)
            {
                throw new ArgumentNullException(nameof(person), $"{typeof(T).Name} cannot be null");
            }
            _persons.Update(person);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteAsync(Guid id)
        {
            var person = await _persons.FindAsync(id);
            if (person is not null)
            {
                _persons.Remove(person);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} found with ID: {id}");
            }
        }

        public async Task<T> GetByAccountIdAsync(Guid accountId)
        {
            if (accountId.ToString() is null)
            {
                throw new ArgumentNullException($"{nameof(accountId)}, for {typeof(T).Name} cannot be null");
            }

            var account = await _persons.FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (account is null)
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} not found with ID: {accountId}");
            }

            return account;
        }
    }
}
