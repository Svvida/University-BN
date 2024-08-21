namespace Domain.Interfaces
{
    public interface IPasswordResetTokenStore
    {
        string GenerateToken(string email);
        bool ValidateToken(string token, string email);
        void InvalidateToken(string token);
    }
}
