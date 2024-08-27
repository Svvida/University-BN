namespace Domain.Interfaces
{
    public interface IPasswordResetTokenStore
    {
        string GenerateToken(string email, string resetIdentifier);
        bool ValidateToken(string token, string email, string resetIdentifier);
        void InvalidateToken(string token);
    }
}
