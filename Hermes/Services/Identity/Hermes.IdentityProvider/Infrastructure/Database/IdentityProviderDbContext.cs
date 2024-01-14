using Hermes.IdentityProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Infrastructure.Database
{
    public class IdentityProviderDbContext : DbContext
    {
        public IdentityProviderDbContext(DbContextOptions<IdentityProviderDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
