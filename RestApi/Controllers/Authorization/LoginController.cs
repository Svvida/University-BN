using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace RestApi.Controllers.Authorization
{
    [ApiController]
    [Route("/")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly HttpJwtService _httpJwtService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            ILoginService loginService,
            HttpJwtService httpJwtService,
            ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _httpJwtService = httpJwtService;
            _logger = logger;
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

            // Only set the sessionId cookie and save session to database if "Remember Me" is true
            if (sessionId is not null)
            {
                bool rememberMe = loginDto.RememberMe;

                if (rememberMe)
                {
                    _httpJwtService.SetSessionIdCookie(Response, sessionId, DateTime.Now.AddDays(7)); // Persistent cookie
                    _logger.LogInformation("Login successful with 'Remember Me' for user: {Identifier}. Tokens issued.", loginDto.Identifier);
                }
                else
                {
                    _httpJwtService.SetSessionIdCookie(Response, sessionId, null); // Session cookie (expires on browser close)
                    _logger.LogInformation("Login successful without 'Remember Me' for user: {Identifier}. Session cookie issued.", loginDto.Identifier);
                }
            }

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

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (tokenHandler.CanReadToken(jwtToken))
            {
                var token = tokenHandler.ReadJwtToken(jwtToken);
                var userIdFromToken = token.Claims.First(claim => claim.Type == "userId")?.Value;
                _logger.LogInformation($"Extracted User ID directly from JWT: {userIdFromToken}");
            }

            var userId = User.FindFirst("userId")?.Value;  // Retrieve userId from JWT claims
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID could not be retrieved from the JWT.");
                return Unauthorized("Invalid user information.");
            }

            var (newToken, newSessionId) = await _loginService.RefreshTokenAsync(sessionId, null); // Adjusted for the centralized logic

            if (newToken == null)
            {
                _logger.LogWarning("Token refresh failed for session ID: {SessionId}", sessionId);
                return Unauthorized("Invalid session ID or token.");
            }

            _logger.LogInformation("Token refreshed successfully. New access token issued.");

            return Ok(new { accessToken = newToken });
        }


        [HttpPost("postAuth/logout")]
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
