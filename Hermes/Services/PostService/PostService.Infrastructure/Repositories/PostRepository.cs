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

        public virtual void Create(Post entity)
        {
            if (entity == null) 
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.PostsCollection.InsertOne(entity);
        }

        public async Task<Post> GetFirstOrDefaultAsync(Expression<Func<Post, bool>> where, params Expression<Func<Post, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, id);
            var result = await _context.PostsCollection.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ArgumentException("Not Found!");
            }
            return result;
        }
    }
}
