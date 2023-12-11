using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace UserService.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("UserServiceConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>()
                .UseNpgsql(connectionString);

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}
