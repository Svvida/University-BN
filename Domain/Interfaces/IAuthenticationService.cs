namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        bool ValidateUser(string username, string password);
    }
}
