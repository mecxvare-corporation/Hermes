﻿using Microsoft.EntityFrameworkCore;
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

            if (!_context.Users.Any())
            {
                var defaultImage = "8A16EDA0-C093-4346-B313-335F83C0B02E_default.jpg";

                var user = new User(Guid.NewGuid(), "John", "Doe", DateTime.UtcNow.AddYears(-25));
                user.SetImageUri(defaultImage);

                _context.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var admin = new User(Guid.NewGuid(), "Admin", "Admin", DateTime.UtcNow);
                var dato = new User(Guid.NewGuid(), "Dato", "Abesalashvili", DateTime.UtcNow);
                var gvantsa = new User(Guid.NewGuid(), "Gvantsa", "Maisuradze", DateTime.UtcNow);
                var bacho = new User(Guid.NewGuid(), "Bacho", "Gigineishvili", DateTime.UtcNow);
                var dama = new User(Guid.NewGuid(), "Giorgi", "Didebulidze", DateTime.UtcNow);

                var userList = new List<User>
                {
                    admin,
                    dato,
                    gvantsa,
                    bacho,
                    dama
                };

                dato.AddFriend(gvantsa);

                dato.AddFriend(bacho);

                bacho.AddFriend(dama);

                dama.AddFriend(dato);

                await _context.AddRangeAsync(userList);

                await _context.SaveChangesAsync();
            }
        }
    }
}
