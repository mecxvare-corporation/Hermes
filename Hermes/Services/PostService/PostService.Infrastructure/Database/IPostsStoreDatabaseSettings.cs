namespace PostService.Infrastructure.Database
{
    public interface IPostsStoreDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; } 

        public string PostsCollectionName { get; set; }
    }
}
