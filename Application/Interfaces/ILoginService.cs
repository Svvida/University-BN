using Application.DTOs;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
