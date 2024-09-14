using Domain.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Services.MongoLogging
{
    public class MongoDbLoggerService : IMongoLogger
    {
        private readonly IMongoCollection<LogEntry> _logInfoCollection;
        private readonly IMongoCollection<LogEntry> _logWarnCollection;

        public MongoDbLoggerService(IMongoDatabase database)
        {
            _logInfoCollection = database.GetCollection<LogEntry>("Informations");
            _logWarnCollection = database.GetCollection<LogEntry>("Warnings");
        }

        public void LogInfo(string message, string? userId)
        {
            var log = new LogEntry
            {
                UserId = string.IsNullOrEmpty(userId) ? null : userId,
                Message = message,
                TimeStamp = DateTime.UtcNow
            };
            _logInfoCollection.InsertOne(log);
        }

        public void LogWarn(string message, string? userId)
        {
            var log = new LogEntry
            {
                UserId = string.IsNullOrEmpty(userId) ? null : userId,
                Message = message,
                TimeStamp = DateTime.UtcNow
            };
            _logWarnCollection.InsertOne(log);
        }
    }
}
