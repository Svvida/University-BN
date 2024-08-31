﻿using Application.DTOs;
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
        private readonly ITokenManager _tokenManager;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<LoginService> _logger;

        public LoginService(
            IAuthenticationService authenticationService,
            ITokenManager tokenManager,
            IAccountRepository accountRepository,
            ILogger<LoginService> logger)
        {
            _authenticationService = authenticationService;
            _tokenManager = tokenManager;
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
            var token = _tokenManager.GenerateAccessToken(user);

            string sessionId = null;

            if (loginDto.RememberMe)
            {
                // Generate session ID and save session & refresh token using TokenManager
                sessionId = _tokenManager.GenerateSessionId();
                var refreshToken = _tokenManager.GenerateRefreshToken();
                _tokenManager.StoreSession(user.Id.ToString(), sessionId, refreshToken, loginDto.RememberMe);

                _logger.LogInformation("User logged in with 'Remember Me'. Session ID and refresh token saved in memory.");
            }
            else
            {
                _logger.LogInformation("User logged in without 'Remember Me'. No session ID or refresh token stored.");
            }

            return (token, sessionId);
        }

        public async Task<(string token, string sessionId)> RefreshTokenAsync(string sessionId, string refreshToken)
        {
            _logger.LogInformation("Attempting to refresh token with session ID: {SessionId}", sessionId);

            // Refresh the access token using TokenManager
            var newToken = await _tokenManager.RefreshAccessTokenAsync(sessionId);

            if (newToken == null)
            {
                _logger.LogWarning("Failed to refresh token. Session or refresh token might be invalid or expired.");
                return (null, null);
            }

            _logger.LogInformation("Generated new access token for Session ID: {SessionId}", sessionId);

            return (newToken, sessionId);
        }
        public void Logout(string sessionId)
        {
            _logger.LogInformation("Logging out session with sessionId: {SessionId}", sessionId);
            _tokenManager.InvalidateSession(sessionId);
            _logger.LogInformation("Session invalidated successfully.");
        }
    }
}
