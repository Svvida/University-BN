using Domain.Entities.AccountEntities;
using Domain.Enums;
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

        public async Task<IEnumerable<UserAccount>> GetByFieldAsync(AccountSearchableFields field, string value)
        {
            string dbFieldName = GetDbFieldName(field);
            var accounts = await _context.UsersAccounts.Where(e => EF.Property<string>(e, dbFieldName) == value).ToListAsync();

            if (!accounts.Any())
            {
                throw new KeyNotFoundException($"No Account fount with {field} = {value}");
            }

            return accounts;
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
                await _context.SaveChangesAsync();
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
                await _context.SaveChangesAsync();
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
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }

        public async Task<UserAccount> GetByUsername(string username)
        {

            var account = await _context.UsersAccounts.FirstOrDefaultAsync(e => e.Login == username);
            if (account is not null)
            {
                return account;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetAllUsernames()
        {
            return _context.UsersAccounts.Select(ua => ua.Login).ToList();
        }
    }
}
