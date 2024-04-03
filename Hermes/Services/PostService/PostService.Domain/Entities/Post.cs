using MongoDB.Bson.Serialization.Attributes;

namespace PostService.Domain.Entities
{
    public class Post : Entity
    {
        [BsonElement("content")]
        public string Content { get; private set; }
        // TODO: Add other needed properties
        //Question: What to do with User? Does we need User here?

        public Post()
        {
            
        }

        public Post(string content)
        {
            Id = Guid.NewGuid();
            Content = content;
        }
    }
}
