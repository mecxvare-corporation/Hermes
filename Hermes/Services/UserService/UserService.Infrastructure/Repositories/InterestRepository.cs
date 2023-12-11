using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class InterestRepository : Repository<Interest>, IInterestRepository
    {
        public InterestRepository(UserDbContext dbContext) : base(dbContext)
        {
        }
    }
}
