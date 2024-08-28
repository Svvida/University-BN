using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRefreshTokenStore
    {
        void StoreToken(string userId, string sessionId, string refreshToken, DateTime expiry);
        bool ValidateToken(string userId, string sessionId, string refreshToken);
        void InvalidateToken(string userId, string sessionId);
    }
}
