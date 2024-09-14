namespace Domain.Interfaces
{
    public interface IMongoLogger
    {
        void LogInfo(string message, string? userId);
        void LogWarn(string message, string? userId);
    }
}
