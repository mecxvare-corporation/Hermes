using Microsoft.Extensions.Options;

using MongoDB.Driver;

using PostService.Api.Infrastructure.MongoDbSettings;
using PostService.Domain.Entities;

namespace PostService.Infrastructure.Database
{
    public class PostsStoreDatabase
    {
        private readonly IMongoCollection<Post> _postsCollection;

        public PostsStoreDatabase(IOptions<PostsStoreDatabaseSettings> settings, IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _postsCollection = mongoDatabase.GetCollection<Post>(settings.Value.PostsCollectionName);
        }

        public IMongoCollection<Post> PostsCollection => _postsCollection;
    }
}

