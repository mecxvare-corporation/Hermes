﻿using PostService.Infrastructure.Database;

namespace PostService.Api.Infrastructure.MongoDbSettings
{
    public class PostsStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PostsCollectionName { get; set; } = null!;
    }
}
