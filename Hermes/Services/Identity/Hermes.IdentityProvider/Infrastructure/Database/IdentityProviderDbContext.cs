using Hermes.IdentityProvider.Entities;
using Hermes.IdentityProvider.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Infrastructure.Database
{
    public class IdentityProviderDbContext : DbContext
    {
        public IdentityProviderDbContext(DbContextOptions<IdentityProviderDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserClaim> UserClaims { get; set; }

        public DbSet<RegisterUserSagaData> RegisterUserSagaData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserClaims)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RegisterUserSagaData>().HasKey(s => s.CorrelationId);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
