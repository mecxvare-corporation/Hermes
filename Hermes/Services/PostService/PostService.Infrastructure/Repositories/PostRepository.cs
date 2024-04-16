using MongoDB.Driver;

using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Database;

using System.Linq.Expressions;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly PostsStoreDatabase _context;

        public PostRepository(PostsStoreDatabase context)
        {
            _context = context;
        }

        public async Task<Post> GetFirstOrDefaultAsync(Expression<Func<Post, bool>> filter)
        {
            var post = await _context.PostsCollection.Find(filter).FirstOrDefaultAsync();

            if (post == null)
            {
                throw new ArgumentException("Not Found!");
            }

            return post;
        }

        public async Task<Post> CreateAsync(Post entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.PostsCollection.InsertOneAsync(entity);

            return entity;
        }

        public async Task<bool> UpdateAsync(Post entity)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, entity.Id);
            var result = await _context.PostsCollection.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ArgumentException("Not Found!");
            }

            await _context.PostsCollection.ReplaceOneAsync(filter, entity);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, id);
            var result = await _context.PostsCollection.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ArgumentException("Not Found!");
            }

            await _context.PostsCollection.DeleteOneAsync(filter);

            return true;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var allPosts = await _context.PostsCollection.Find(Builders<Post>.Filter.Empty).ToListAsync();
            return allPosts;
        }

        public async Task<List<Post>> GetAllAsync(Guid userId)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.UserId, userId);
            var allPosts = await _context.PostsCollection.Find(filter).ToListAsync();

            return allPosts;
        }
    }
}
