using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Models;
using Hermes.IdentityProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Infrastructure.Database
{
    public class IdentityDbInitializer
    {
        private readonly ILogger<IdentityProviderDbContext> _logger;
        private readonly IdentityProviderDbContext _context;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;

        public IdentityDbInitializer(ILogger<IdentityProviderDbContext> logger, IdentityProviderDbContext context, ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext)
        {
            _logger = logger;
            _context = context;
            _configurationDbContext = configurationDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsNpgsql())
                {
                    await _context.Database.MigrateAsync();
                }
                if (_configurationDbContext.Database.IsNpgsql())
                {
                    await _configurationDbContext.Database.MigrateAsync();
                }
                if (_persistedGrantDbContext.Database.IsNpgsql())
                {
                    await _persistedGrantDbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            await SeedUsers();
        }

        private async Task SeedUsers()
        {
            var users = new List<User>
            {
                new User { UserName = "admin", Email = "admin@hermes.ge", Password = "password".Sha256()}.AddUserClaims(new UserClaim ("role", "admin")),
                new User { UserName = "dev", Email = "dev@hermes.ge", Password = "password".Sha256()}.AddUserClaims(new UserClaim ("role", "dev")),
            };

            await _context.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }
    }

}

