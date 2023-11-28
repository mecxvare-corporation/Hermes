using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserService.Infrastructure.Database
{
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
                .UseSqlServer(connectionString);

            return new UserDbContext(optionsBuilder.Options); //srvers ver ukavshirdeba vnaxot mere ra xdeba
        }
    }
}
