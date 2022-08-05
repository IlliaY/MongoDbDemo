using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDemo.Entities
{
    public interface IBaseEntity
    {
        [BsonId]
        Guid Id { get; set; }
    }
}