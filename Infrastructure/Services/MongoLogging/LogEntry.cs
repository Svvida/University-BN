using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Services.MongoLogging
{
    public class LogEntry
    {
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        public string? UserId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
