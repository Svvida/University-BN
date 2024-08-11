using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.ExternalEntities;

namespace Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly UniversityContext _context;

        public CompanyRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<Company> GetByAccountIdAsync(Guid accountId)
        {
            if (accountId.ToString() is null)
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            var account = await _context.Companies.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if(account is null)
            {
                throw new KeyNotFoundException($"No Account found with ID: {accountId}");
            }

            return account;
        }
    }
}
