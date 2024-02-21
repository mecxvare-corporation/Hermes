using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Infrastructure.Database
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
    }
}
