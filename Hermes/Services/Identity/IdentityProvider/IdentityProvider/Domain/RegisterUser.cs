using IdentityModel;
using IdentityProvider.Entities;
using IdentityProvider.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Domain
{
    public class RegisterUser
    {
        private readonly IdentityDbContext _dbContext;

        public RegisterUser(IdentityDbContext dbContext)
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
