using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly HttpJwtService _httpJwtService;

        public LoginController(ILoginService loginService, HttpJwtService httpJwtService)
        {
            _loginService = loginService;
            _httpJwtService = httpJwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto is null || string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Invalid login request");
            }

            var (token, refreshToken) = await _loginService.LoginAsync(loginDto);
            if (token is null)
            {
                return Unauthorized("Invalid username or password");
            }

            _httpJwtService.SetRefreshTokenCookie(Response, refreshToken, DateTime.Now.AddDays(1));

            return Ok(new { Token = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = _httpJwtService.GetRefreshTokenFromCookies(Request);
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token provided.");
            }

            var (newToken, newRefreshToken) = await _loginService.RefreshTokenAsync(refreshToken);

            if (newToken is null)
            {
                return Unauthorized("Invalid refresh token");
            }

            _httpJwtService.SetRefreshTokenCookie(Response, newRefreshToken, DateTime.Now.AddDays(7));

            return Ok(new { Token = newToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _httpJwtService.RemoveRefreshTokenCookie(Response);
            return Ok("Logged out");
        }
    }
}
