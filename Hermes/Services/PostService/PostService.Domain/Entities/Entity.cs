using MongoDB.Bson.Serialization.Attributes;

namespace PostService.Domain.Entities
{
    public class Entity
    {
        [BsonId]
        public Guid Id { get; protected set; }
    }
}
