using Application.DTOs;
using Application.Interfaces;
using Domain.Enums.SearchableFields;
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

            var user = await _accountRepository.GetByFieldAsync(AccountSearchableFields.Login, loginDto.Identifier);
            var token = _jwtService.GenerateToken(user);

            string sessionId = null;

            if (loginDto.RememberMe)
            {
                // Generate session ID and save session & refresh token to the database
                sessionId = Guid.NewGuid().ToString();
                _authenticationService.StoreRefreshToken(user.Id.ToString(), sessionId, _jwtService.GenerateRefreshToken());

                await _accountRepository.UpdateAsync(user);

                _logger.LogInformation("User logged in with 'Remember Me'. Session ID and refresh token saved in database.");
            }
            else
            {
                _logger.LogInformation("User logged in without 'Remember Me'. No session ID or refresh token stored.");
            }

            return (token, sessionId);
        }

        public async Task<(string token, string sessionId)> RefreshTokenAsync(string userId, string sessionId, string refreshToken)
        {
            _logger.LogInformation("Attempting to refresh token with session ID: {SessionId}", sessionId);

            var isValidSession = _authenticationService.ValidateSession(userId, sessionId);
            if (!isValidSession)
            {
                _logger.LogWarning("Session validation failed. Session not found or invalid.");
                return (null, null);
            }

            var isValidToken = _authenticationService.ValidateRefreshToken(userId, sessionId, refreshToken);
            if (!isValidToken)
            {
                _logger.LogWarning("Refresh token expired");
                return (null, null);
            }

            var user = await _accountRepository.GetByIdAsync(new Guid(userId));
            var newToken = _jwtService.GenerateToken(user);

            _logger.LogInformation("Generated new access token for user: {Identifier}. Session ID remains the same: {SessionId}", user.Login, sessionId);

            return (newToken, sessionId);
        }
    }
}
