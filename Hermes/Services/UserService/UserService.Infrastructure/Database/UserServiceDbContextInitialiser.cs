using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
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
            await SeedInterests();
            await SeedUsers();
        }

        public async Task SeedInterests()
        {
            if (!_context.Interests.Any())
            {
                var interestsList = new List<Interest>
                {
                    new Interest("Travel"),
                    new Interest("Food and Cooking"),
                    new Interest("Fitness and Wellness"),
                    new Interest("Fashion"),
                    new Interest("Technology"),
                    new Interest("Books and Literature"),
                    new Interest("Movies and TV Shows"),
                    new Interest("Music"),
                    new Interest("Gaming"),
                    new Interest("Art and Creativity")
                };

                await _context.AddRangeAsync(interestsList);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var esgeso = new User("Esgeso", "Namoradze", DateTime.UtcNow);
                var takhmaGido = new User("Takhma", "Gido", DateTime.UtcNow);
                var zoro = new User("Zoro", "", DateTime.UtcNow);
                var madona = new User("Madona", "", DateTime.UtcNow);
                var mchedeliVano = new User("Mchedeli", "Vano", DateTime.UtcNow);

                var userList = new List<User>
                {
                    esgeso,
                    takhmaGido,
                    zoro,
                    madona,
                    mchedeliVano
                };

                zoro.AddFriend(madona);

                esgeso.AddFriend(mchedeliVano);
                esgeso.AddFriend(takhmaGido);

                await _context.AddRangeAsync(userList);

                await _context.SaveChangesAsync();
            }
        }
    }
}
