using Hermes.IdentityProvider.Entities;
using Hermes.IdentityProvider.Infrastructure.Database;
using IdentityModel;
using MassTransit;
using Messages;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Domain
{
    public class RegisterUser
    {
        private readonly IdentityProviderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterUser(IdentityProviderDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<User> CreateUser(string userName, string email, string Password, string firstName, string lastName, DateTime dateOfBirth)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()) || x.Email.ToLower().Equals(email.ToLower()));

            if (user is null)
            {
                var newUser = new User
                {
                    UserName = userName,
                    Email = email,
                    Password = Password.ToSha256(),
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth
                };

                newUser.AddUserClaims(new UserClaim { ClaimType = "role", ClaimValue = "dev", User = newUser });

                _dbContext.Users.Add(newUser);

                await _publishEndpoint.Publish(new UserRegistered
                {
                    UserId = Guid.Parse(newUser.SubjectId),
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    DateOfBirth = newUser.DateOfBirth
                });

                _dbContext.SaveChanges();

                return newUser;
            }
            else
            {
                throw new Exception("User already exists");
            }
        }
    }
}
