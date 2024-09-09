using Application.DTOs;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        Task<(string token, string sessionId)> LoginAsync(LoginDto loginDto);
    }
}
