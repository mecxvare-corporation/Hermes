using PostService.Domain.Interfaces;
using PostService.Infrastructure.Database;

namespace PostService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PostDbContext _context;
        public IPostRepository PostRepository { get; init; }

        public UnitOfWork(PostDbContext context, IPostRepository postRepository)
        {
            _context = context;
            PostRepository = postRepository;
        }

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
