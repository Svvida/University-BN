using Domain.Entities.ExternalEntities;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> GetByAccountIdAsync(Guid accountId);
    }
}
