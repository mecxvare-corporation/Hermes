using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace IdentityProvider.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("IdentityServiceConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseNpgsql(connectionString);

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
