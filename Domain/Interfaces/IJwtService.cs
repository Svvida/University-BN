using Domain.Entities.AccountEntities;

namespace Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserAccount userAccount);
    }
}
