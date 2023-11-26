using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Database
{
    public class HermesDbContext : DbContext
    {
        public HermesDbContext(DbContextOptions<HermesDbContext> options) : base(options)
        {
        }

        //DBSets
        //public DbSet<YourEntity> YourEntities { get; set; }
    }
}
