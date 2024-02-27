using Hermes.IdentityProvider.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Domain
{
    public class CredentialsValidator
    {
        private readonly IdentityProviderDbContext _dbContext;

        public CredentialsValidator(IdentityProviderDbContext dbContext)
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
