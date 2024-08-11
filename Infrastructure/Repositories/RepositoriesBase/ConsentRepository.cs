using Domain.EntitiesBase;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class ConsentRepository<T> : IConsentRepository<T> where T : ConsentBase
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _consents;

        public ConsentRepository(UniversityContext context)
        {
            _context = context;
            _consents = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var consent = await _consents.FindAsync(id);
            if (consent is not null)
            {
                return consent;
            }
            else
            {
                throw new KeyNotFoundException("Consent not found");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _consents.ToListAsync();
        }

        public async Task CreateAsync(T consent)
        {
            if (consent is null)
            {
                throw new ArgumentNullException(nameof(consent), "Consent cannot be null");
            }
            await _consents.AddAsync(consent);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task UpdateAsync(T consent)
        {
            if (consent is null)
            {
                throw new ArgumentNullException(nameof(consent), "Consent cannot be null");
            }
            _consents.Update(consent);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteAsync(Guid id)
        {
            var consent = await _consents.FindAsync(id);
            if (consent is not null)
            {
                _consents.Remove(consent);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new KeyNotFoundException("Consent not found");
            }
        }

        public async Task SwitchConsentAsync(Guid id, string consentType)
        {
            var consent = await _consents.FindAsync(id);

            if (consent is null)
            {
                throw new KeyNotFoundException("Consent not found");
            }

            switch (consentType.ToLower())
            {
                case "photo":
                    consent.PermissionForPhoto = !consent.PermissionForPhoto;
                    break;
                case "data":
                    consent.PermissionForDataProcessing = !consent.PermissionForDataProcessing;
                    break;
                default:
                    throw new ArgumentException("Invalid consent type", nameof(consentType));
            }

            _consents.Update(consent);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
