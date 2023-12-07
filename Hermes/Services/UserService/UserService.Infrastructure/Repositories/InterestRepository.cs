using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Repositories
{
    public class InterestRepository : Repository<Interest>, IInterestRepository
    {
        public InterestRepository(UserDbContext dbContext) : base(dbContext)
        {
        }
    }
}
