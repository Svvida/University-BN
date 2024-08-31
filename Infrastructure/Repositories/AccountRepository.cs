using Domain.Entities.AccountEntities;
using Domain.Enums.SearchableFields;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UniversityContext _context;

        public AccountRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<UserAccount> GetByIdAsync(Guid id)
        {
            var account = await _context.UsersAccounts.FindAsync(id);
            if (account is not null)
            {
                return account;
            }
            else
            {
                throw new KeyNotFoundException("Account not fount");
            }
        }

        public async Task<IEnumerable<UserAccount>> GetAllAsync()
        {
            return await _context.UsersAccounts.ToListAsync();
        }

        public async Task<UserAccount> GetByFieldAsync(AccountSearchableFields field, string value)
        {
            string dbFieldName = GetDbFieldName(field);
            var account = await _context.UsersAccounts.Where(e => EF.Property<string>(e, dbFieldName) == value).FirstOrDefaultAsync();

            if (account is null)
            {
                throw new KeyNotFoundException($"No Account fount with {field} = {value}");
            }

            return account;
        }

        private string GetDbFieldName(AccountSearchableFields field)
        {
            switch (field)
            {
                case AccountSearchableFields.Login:
                    return "Login";
                case AccountSearchableFields.Email:
                    return "Email";
                default:
                    throw new ArgumentException("Invalid field", nameof(field));

            }
        }

        public async Task CreateAsync(UserAccount account)
        {
            if (account is not null)
            {
                await _context.UsersAccounts.AddAsync(account);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new ArgumentNullException("Account cannot be null");
            }
        }

        public async Task UpdateAsync(UserAccount account)
        {
            if (account is not null)
            {
                _context.UsersAccounts.Update(account);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new ArgumentNullException("Account cannot be null");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var account = await _context.UsersAccounts.FindAsync(id);
            if (account is not null)
            {
                _context.UsersAccounts.Remove(account);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }

        public IEnumerable<string> GetAllUsernames()
        {
            return _context.UsersAccounts.Select(ua => ua.Login).ToList();
        }
    }
}
