using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        Task<(string token, string refreshToken)> LoginAsync(LoginDto loginDto);
        Task<(string token, string refreshToken)> RefreshTokenAsync(string refreshToken);
    }
}
