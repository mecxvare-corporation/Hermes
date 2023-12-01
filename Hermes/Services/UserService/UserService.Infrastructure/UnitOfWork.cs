using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserDbContext _context;
        public IUserRepository UserRepository { get; init; }

        public UnitOfWork(UserDbContext context, IUserRepository userRepository)
        {
            _context = context;
            UserRepository = userRepository;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
