using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers.Authorization
{
    [ApiController]
    [Route("/")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly HttpJwtService _httpJwtService;
        private readonly ILogger<LoginController> _logger;
        private readonly ITokenManager _tokenManager;
        private readonly IMongoLogger _mongoLogger;

        public LoginController(
            ILoginService loginService,
            HttpJwtService httpJwtService,
            ILogger<LoginController> logger,
            ITokenManager tokenManager,
            IMongoLogger mongoLogger)
        {
            _loginService = loginService;
            _httpJwtService = httpJwtService;
            _logger = logger;
            _tokenManager = tokenManager;
            _mongoLogger = mongoLogger;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto is null || string.IsNullOrWhiteSpace(loginDto.Identifier) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                _logger.LogWarning("Invalid login request received.");
                return BadRequest("Invalid login request");
            }

            _logger.LogInformation("Attempting to login user with identifier: {Identifier}", loginDto.Identifier);
            var (token, sessionId) = await _loginService.LoginAsync(loginDto);

            if (token is null)
            {
                _logger.LogWarning("Login failed for user: {Identifier}", loginDto.Identifier);
                _mongoLogger.LogWarn($"Login failed for user: {loginDto.Identifier}", null);
                return Unauthorized("Invalid username or password");
            }

            // Only set the sessionId cookie and save session to database if "Remember Me" is true
            if (sessionId is not null)
            {
                bool rememberMe = loginDto.RememberMe;

                if (rememberMe)
                {
                    _httpJwtService.SetSessionIdCookie(Response, sessionId, DateTime.Now.AddDays(7)); // Persistent cookie
                    _logger.LogInformation("Login successful with 'Remember Me' for user: {Identifier}. Tokens issued.", loginDto.Identifier);
                    _mongoLogger.LogInfo($"Login successful with 'Remember Me' for user {loginDto.Identifier}. Tokens issued", null);
                }
                else
                {
                    _httpJwtService.SetSessionIdCookie(Response, sessionId, null); // Session cookie (expires on browser close)
                    _logger.LogInformation("Login successful without 'Remember Me' for user: {Identifier}. Session cookie issued.", loginDto.Identifier);
                    _mongoLogger.LogInfo($"Login successful without 'Remember Me' for user: {loginDto.Identifier}. Session cookie issued.", null);
                }
            }

            return Ok(new { accessToken = token });
        }

        [HttpPost("auth/refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
        {
            if (!Request.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Unsupported Content-Type: {Request.ContentType}");
                return NoContent();
            }

            _logger.LogInformation("Attempting to refresh token.");

            var sessionId = _httpJwtService.GetSessionIdFromCookies(Request);
            if (string.IsNullOrEmpty(sessionId))
            {
                _logger.LogWarning("No session ID provided in the request.");
                return Unauthorized("No session ID provided.");
            }

            var newAccessToken = await _tokenManager.RefreshAccessTokenAsync(sessionId.ToString());
            if (newAccessToken == null)
            {
                _logger.LogWarning("Failed to refresh token. Session might be invalid or expired.");
                return Unauthorized("Invalid session or token.");
            }

            _logger.LogInformation($"Token refreshed successfully for sessionId: {sessionId}");

            return Ok(new { accessToken = newAccessToken });
        }


        [HttpPost("auth/logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation("User logged out, removing session ID cookie.");
            var sessionId = _httpJwtService.GetSessionIdFromCookies(Request);
            if (!string.IsNullOrEmpty(sessionId))
            {
                _loginService.Logout(sessionId);
            }

            _httpJwtService.RemoveSessionIdCookie(Response);
            return Ok("Logged out");
        }
    }
}
