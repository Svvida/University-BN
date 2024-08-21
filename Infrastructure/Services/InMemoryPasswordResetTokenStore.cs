using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class InMemoryPasswordResetTokenStore : IPasswordResetTokenStore
    {
        private readonly ConcurrentDictionary<string, (string email, DateTime expiry)> _tokens
            = new ConcurrentDictionary<string, (string email, DateTime expiry)>();
        private readonly ILogger<InMemoryPasswordResetTokenStore> _logger;

        public InMemoryPasswordResetTokenStore(ILogger<InMemoryPasswordResetTokenStore> logger)
        {
            _logger = logger;
        }

        public string GenerateToken(string email)
        {
            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddHours(1);
            _logger.LogInformation($"Generated reset password token: {token} that expires: {expiry} for account: {email}");
            _tokens[token] = (email, expiry);

            return token;
        }

        public bool ValidateToken(string token, string email)
        {
            if (_tokens.TryGetValue(token, out var tokenData))
            {
                if (tokenData.email == email && tokenData.expiry > DateTime.UtcNow)
                {
                    return true;
                }

                _tokens.TryRemove(token, out _);
            }
            return false;
        }

        public void InvalidateToken(string token)
        {
            _tokens.TryRemove(token, out _);
        }
    }
}
