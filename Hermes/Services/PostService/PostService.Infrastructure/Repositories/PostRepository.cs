using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Database;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(PostDbContext context) : base(context)
        {
        }
    }
}
