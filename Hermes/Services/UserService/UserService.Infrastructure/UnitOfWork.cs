using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserDbContext _context;
        public IUserRepository UserRepository { get; init; }
        public IInterestRepository InterestRepository { get; init; }

        public UnitOfWork(UserDbContext context, IUserRepository userRepository, IInterestRepository interestRepository)
        {
            _context = context;
            UserRepository = userRepository;
            InterestRepository = interestRepository;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
