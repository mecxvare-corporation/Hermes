using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Interest> Interests { get; set; }
    }
}
