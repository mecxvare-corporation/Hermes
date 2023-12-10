using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UserDbContext dbContext) : base(dbContext)
        {
        }
    }
}
