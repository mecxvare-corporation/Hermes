using MongoDB.Bson.Serialization.Attributes;

namespace PostService.Domain.Entities
{
    public class Post : Entity
    {
        [BsonElement("content")]
        public string Content { get; private set; }
        // TODO: Add other needed properties
        //Question: What to do with User? Does we need User here? anu posts tu qmnis vigaca avtorizebulia anu. eseigi magis aidi xo gaq frontshi hoda gamoatan mag aidis da ise sheqmni posts aa ok gavige

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
