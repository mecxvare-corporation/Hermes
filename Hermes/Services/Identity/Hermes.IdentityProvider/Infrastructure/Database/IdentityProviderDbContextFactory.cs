using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace Hermes.IdentityProvider.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
    public class IdentityProviderDbContextFactory : IDesignTimeDbContextFactory<IdentityProviderDbContext>
    {
        public IdentityProviderDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("UserServiceConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<IdentityProviderDbContext>()
                .UseNpgsql(connectionString);

            return new IdentityProviderDbContext(optionsBuilder.Options);
        }
    }
}
