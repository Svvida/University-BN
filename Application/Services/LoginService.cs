using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;

        public LoginService(IAuthenticationService authenticationService, IJwtService jwtService)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
        }

        public async Task<(string token, string refreshToken)> LoginAsync(LoginDto loginDto)
        {
            var isValidated = await _authenticationService.ValidateUserAsync(loginDto.Username, loginDto.Password);
            if (!isValidated)
            {
                return (null, null);
            }

            var user = await _authenticationService.GetUserAsync(loginDto.Username);
            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return (token, refreshToken);
        }

        public async Task<(string token, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _authenticationService.ValidateRefreshTokenAsync(refreshToken);
   
            if(user is null)
            {
                return (null, null);
            }

            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            return (newToken, newRefreshToken);
        }
    }
}
