using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class InMemoryPasswordResetTokenStore : IPasswordResetTokenStore
    {
        private readonly ConcurrentDictionary<string, (string email, DateTime expiry, string resetIdentifier)> _tokens
            = new ConcurrentDictionary<string, (string email, DateTime expiry, string resetIdentifier)>();
        private readonly ILogger<InMemoryPasswordResetTokenStore> _logger;

        public InMemoryPasswordResetTokenStore(ILogger<InMemoryPasswordResetTokenStore> logger)
        {
            _logger = logger;
        }

        public string GenerateToken(string email, string resetIdentifier)
        {
            string token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddMinutes(30);

            _tokens[token] = (email, expiry, resetIdentifier);

            _logger.LogInformation($"Generated reset token: {token} for email: {email} with expiry: {expiry} and associated with identifier: {resetIdentifier}");

            return token;
        }

        public bool ValidateToken(string token, string email, string resetIdentifier)
        {
            if (_tokens.TryGetValue(token, out var tokenData))
            {
                if (tokenData.email == email && tokenData.expiry > DateTime.UtcNow && tokenData.resetIdentifier == resetIdentifier)
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
