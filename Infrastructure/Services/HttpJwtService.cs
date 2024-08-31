using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class HttpJwtService
    {
        private readonly ILogger<HttpJwtService> _logger;

        public HttpJwtService(ILogger<HttpJwtService> logger)
        {
            _logger = logger;
        }

        public void SetSessionIdCookie(HttpResponse response, string sessionId, DateTime? expiry)
        {
            _logger.LogInformation("Setting session ID cookie with expiry: {Expiry}", expiry);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiry
            };

            response.Cookies.Append("sessionId", sessionId, cookieOptions);
            _logger.LogInformation("Session ID cookie set successfully.");
        }

        public string GetSessionIdFromCookies(HttpRequest request)
        {
            _logger.LogInformation("Retrieving session ID from cookies.");
            var sessionId = request.Cookies["sessionId"];
            if (string.IsNullOrEmpty(sessionId))
            {
                _logger.LogWarning("Session ID not found in cookies.");
            }
            else
            {
                _logger.LogInformation("Session ID retrieved from cookies: {SessionId}", sessionId);
            }
            return sessionId;
        }

        public void RemoveSessionIdCookie(HttpResponse response)
        {
            _logger.LogInformation("Removing session ID cookie.");
            response.Cookies.Delete("sessionId");
            _logger.LogInformation("Session ID cookie removed successfully.");
        }
    }
}
