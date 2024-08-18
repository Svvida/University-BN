using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountRepository _accountRepository;

        public LoginController(
            ILoginService loginService,
            HttpJwtService httpJwtService,
            ILogger<LoginController> logger,
            IAuthenticationService authenticationService,
            IAccountRepository accountRepository)
        {
            _loginService = loginService;
            _httpJwtService = httpJwtService;
            _logger = logger;
            _authenticationService = authenticationService;
            _accountRepository = accountRepository;
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
                return Unauthorized("Invalid username or password");
            }

            _httpJwtService.SetSessionIdCookie(Response, sessionId, DateTime.Now.AddDays(7));
            _logger.LogInformation("Login successful for user: {Identifier}. Tokens issued.", loginDto.Identifier);

            return Ok(new { accessToken = token });
        }

        [HttpGet("auth/refresh")]
        public async Task<IActionResult> Refresh()
        {
            _logger.LogInformation("Attempting to refresh token.");
            var sessionId = _httpJwtService.GetSessionIdFromCookies(Request);
            if (string.IsNullOrEmpty(sessionId))
            {
                _logger.LogWarning("No session ID provided in the request.");
                return Unauthorized("No session ID provided.");
            }

            _logger.LogInformation("Session ID retrieved: {SessionId}", sessionId);
            var (newToken, _) = await _loginService.RefreshTokenAsync(sessionId);

            if (newToken is null)
            {
                _logger.LogWarning("Invalid session ID provided: {SessionId}", sessionId);
                return Unauthorized("Invalid session ID");
            }

            _logger.LogInformation("Token refreshed successfully. New access token issued.");

            return Ok(new { accessToken = newToken });
        }

        [HttpPost("postAuth/logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User logged out, removing session ID cookie.");
            var sessionId = _httpJwtService.GetSessionIdFromCookies(Request);
            if (string.IsNullOrEmpty(sessionId))
            {
                var user = await _authenticationService.ValidateSessionAsync(new Guid(sessionId));
                if (user is not null)
                {
                    // Invalidate session and refresh token
                    user.SessionId = null;
                    user.RefreshToken = null;
                    await _accountRepository.UpdateAsync(user);
                }
            }
            _httpJwtService.RemoveSessionIdCookie(Response);
            return Ok("Logged out");
        }
    }
}
