using Domain.Entities.AccountEntities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(account is not null)
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

        public async Task<IEnumerable<UserAccount>> GetByFieldAsync(string field, string value)
        {
            var accounts = await _context.UsersAccounts.Where(e => EF.Property<string>(e, field) == value).ToListAsync();
            if (accounts.Any())
            {
                return accounts;
            }
            else
            {
                throw new KeyNotFoundException($"No Account fount with {field} = {value}");
            }
        }

        public async Task CreateAsync(UserAccount account)
        {
            if(account is not null)
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
            if(account is not null)
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
            if(account is not null)
            {
                _context.UsersAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }
    }
}
