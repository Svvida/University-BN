using Domain.Interfaces;

namespace RestApi.Middlewares
{
    public class SessionActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<SessionActivityMiddleware> _logger;

        public SessionActivityMiddleware(
            RequestDelegate next,
            ITokenManager tokenManager,
            ILogger<SessionActivityMiddleware> logger)
        {
            _next = next;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("sessionId", out var sessionId))
            {
                if (_tokenManager.ValidateSession(sessionId, out _))
                {
                    var session = _tokenManager.GetSession(sessionId);
                    if (session is not null || !session.RememberMe)
                    {
                        _tokenManager.UpdateLastActivity(sessionId);
                        _logger.LogInformation($"Updated LastActivity for sessionId: {sessionId}");
                    }
                }
            }

            await _next(context);
        }
    }
}
