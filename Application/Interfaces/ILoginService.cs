using Application.DTOs;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        Task<(string token, string sessionId)> LoginAsync(LoginDto loginDto);
        Task<(string token, string sessionId)> RefreshTokenAsync(string userId, string sessionId, string refreshToken);
    }
}
