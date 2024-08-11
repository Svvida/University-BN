using Domain.Entities.AccountEntities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UniversityContext _context;

        public RoleRepository(UniversityContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role is not null)
            {
                return role;
            }
            else
            {
                throw new KeyNotFoundException("Role not found");
            }
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task CreateAsync(Role role)
        {
            if (role is not null)
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new ArgumentNullException("Role cannot be null");
            }
        }

        public async Task UpdateAsync(Role role)
        {
            if (role is not null)
            {
                _context.Roles.Update(role);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else { throw new ArgumentNullException("Role cannot be null"); }
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role is not null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                throw new KeyNotFoundException("Role not found");
            }
        }
    }
}
