using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserService.Infrastructure.Database
{
    public class UserServiceDbContextInitialiser
    {
        private readonly ILogger<UserDbContext> _logger;
        private readonly UserDbContext _context;

        public UserServiceDbContextInitialiser(ILogger<UserDbContext> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsNpgsql())
                {
                    await _context.Database.MigrateAsync();
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
            //TODO seeding if needed
        }
    }
}
