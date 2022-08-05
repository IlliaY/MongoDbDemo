using MongoDbDemo.Attributes;

namespace MongoDbDemo.Entities
{
    [BsonCollection("people")]
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}