using Hermes.IdentityProvider.Entities;
using Hermes.IdentityProvider.Infrastructure.Database;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Domain
{
    public class RegisterUser
    {
        private readonly IdentityProviderDbContext _dbContext;

        public RegisterUser(IdentityProviderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUser(string userName, string email, string Password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()) || x.Email.ToLower().Equals(email.ToLower()));

            if (user is null)
            {
                var newUser = new User
                {
                    UserName = userName,
                    Email = email,
                    Password = Password.ToSha256()
                };

                newUser.AddUserClaims(new UserClaim { ClaimType = "role", ClaimValue = "dev", User = newUser });

                return newUser;
            }
            else
            {
                throw new Exception("User already exists");
            }
        }
    }
}
