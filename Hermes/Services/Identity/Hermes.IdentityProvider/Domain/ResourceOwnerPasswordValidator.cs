using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Hermes.IdentityProvider.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hermes.IdentityProvider.Domain
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IdentityProviderDbContext _dbContext;

        public ResourceOwnerPasswordValidator(IdentityProviderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == context.UserName.ToLower());

                var validateCredentials = await new CredentialsValidator(_dbContext).ValidateAsync(context.UserName, context.Password);

                if (validateCredentials.IsValid)
                {
                    //set the result
                    context.Result = new GrantValidationResult(
                        subject: user.SubjectId.ToString(),
                        authenticationMethod: "custom",
                        claims: await GetUserClaims(user.SubjectId));

                    return;
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid Credetials");

                return;
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ex.Message);
            }
        }

        private async Task<Claim[]> GetUserClaims(string subjectId)
        {
            var data = await _dbContext.UserClaims.Where(x => x.SubjectId == subjectId).ToListAsync();

            var claimsList = new List<Claim>();

            foreach (var item in data)
            {
                claimsList.Add(new Claim(item.ClaimType, item.ClaimValue));
            }

            return claimsList.ToArray();
        }
    }
}
