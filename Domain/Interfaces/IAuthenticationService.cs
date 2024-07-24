namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUserAsync(string username, string password);
    }
}
