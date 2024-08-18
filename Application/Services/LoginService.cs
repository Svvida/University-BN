using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IAuthenticationService authenticationService, IJwtService jwtService, IAccountRepository accountRepository, ILogger<LoginService> logger)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<(string token, string sessionId)> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Attempting to log in user with identifier: {Identifier}", loginDto.Identifier);

            var isValidated = await _authenticationService.ValidateUserAsync(loginDto.Identifier, loginDto.Password);
            if (!isValidated)
            {
                _logger.LogWarning("Login failed for user: {Identifier}. Invalid credentials.", loginDto.Identifier);
                return (null, null);
            }

            var user = await _authenticationService.GetUserAsync(loginDto.Identifier);
            var token = _jwtService.GenerateToken(user);

            // Generate a new session ID
            var sessionId = Guid.NewGuid().ToString();

            // Store session ID and refresh token in the database
            user.SessionId = new Guid(sessionId);
            user.RefreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _accountRepository.UpdateAsync(user);

            _logger.LogInformation("User logged in successfully. Session ID: {SessionId}, Refresh token stored in database.", sessionId);

            return (token, sessionId);
        }

        public async Task<(string token, string sessionId)> RefreshTokenAsync(string sessionId)
        {
            _logger.LogInformation("Attempting to refresh token with session ID: {SessionId}", sessionId);

            var user = await _authenticationService.ValidateSessionAsync(new Guid(sessionId));

            if (user is null)
            {
                _logger.LogWarning("Session validation failed. Session not found or invalid.");
                return (null, null);
            }

            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token expired for user: {Identifier}. Expiry: {Expiry}", user.Login, user.RefreshTokenExpiryTime);
                return (null, null);
            }

            var newToken = _jwtService.GenerateToken(user);

            _logger.LogInformation("Generated new access token for user: {Identifier}. Session ID remains the same: {SessionId}", user.Login, sessionId);

            return (newToken, sessionId);
        }
    }
}
