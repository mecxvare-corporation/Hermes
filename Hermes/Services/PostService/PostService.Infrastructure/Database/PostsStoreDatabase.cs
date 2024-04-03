using Microsoft.Extensions.Options;

using MongoDB.Driver;

using PostService.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Infrastructure.Database
{
    public class PostsStoreDatabase
    {
        private readonly IMongoCollection<Post> _postsCollection;

        public PostsStoreDatabase(IOptions<IPostsStoreDatabaseSettings> settings, IMongoClient mongoClient)
        {

            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _postsCollection = mongoDatabase.GetCollection<Post>(settings.Value.PostsCollectionName);
        }

        public IMongoCollection<Post> PostsCollection => _postsCollection;
    }
}

