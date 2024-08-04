using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

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

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var isValidated = await _authenticationService.ValidateUserAsync(loginDto.Username, loginDto.Password);
            if (!isValidated)
            {
                return null;
            }

            var user = await _authenticationService.GetUserAsync(loginDto.Username);
            return _jwtService.GenerateToken(user);
        }
    }
}
