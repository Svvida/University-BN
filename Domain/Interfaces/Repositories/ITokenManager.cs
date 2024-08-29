using Domain.Entities.AccountEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITokenManager
    {
        string GenerateAccessToken(UserAccount userAccount);
        string GenerateRefreshToken();
        string GenerateSessionId();
        void StoreSession(string userId, string sessionId, string refreshToken);
        bool ValidateSession(string userId, string sessionId, out string refreshToken);
        void InvalidateSession(string userId, string sessionId);
        Task<string> RefreshAccessTokenAsync(string userId, string sessionId);
    }
}
