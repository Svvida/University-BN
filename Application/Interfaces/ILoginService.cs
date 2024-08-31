using Application.DTOs;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        Task<(string token, string sessionId)> LoginAsync(LoginDto loginDto);
        Task<(string token, string sessionId)> RefreshTokenAsync(string sessionId, string refreshToken);
        void Logout(string sessionId);
    }
}
