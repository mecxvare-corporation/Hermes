using IdentityModel;
using IdentityProvider.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Domain
{
    public class CredentialsValidator
    {
        private readonly IdentityDbContext _dbContext;

        public CredentialsValidator(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CredentialsValidatorResultModel> ValidateAsync(string userName, string Password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower() && x.Password == Password.ToSha256());

            if (user is null)
            {
                return new CredentialsValidatorResultModel { IsValid = false };
            }
            else
            {
                return new CredentialsValidatorResultModel { IsValid = true };
            }
        }
    }

    public class CredentialsValidatorResultModel
    {
        public bool IsValid { get; set; }
    }
}
