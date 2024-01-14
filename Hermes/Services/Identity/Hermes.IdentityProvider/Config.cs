using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;

namespace Hermes.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("userservice", "User Service"),
            new ApiScope("identityprovider", "Identity Provider")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "userservice",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("admin1234".Sha256())
                },

                // scopes that client has access to
                AllowedScopes =
                {
                    "identityprovider"
                }
            }
        };

    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1c9857e4-620d-402d-9a3f-6f7973b3801a",
                Username = "Zoro",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "Zoro"),
                    new Claim(JwtClaimTypes.Role, "Admin")
                }
            },
            new TestUser
            {
                SubjectId = "568c7523-11fb-4d9b-9acb-7f53c1101ace",
                Username = "Esgeso",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "Esgeso"),
                    new Claim(JwtClaimTypes.Role, "User")
                }
            }
        };
}