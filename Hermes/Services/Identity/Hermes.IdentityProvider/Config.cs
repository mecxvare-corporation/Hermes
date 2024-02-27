namespace Hermes.IdentityProvider
{
    using Duende.IdentityServer.Models;

    namespace IdentityProvider
    {
        public static class Config
        {
            public static IEnumerable<IdentityResource> IdentityResources =>
                new IdentityResource[]
                {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() {"role"}),
                 new IdentityResource(
                    "username",
                    "Your username",
                    new List<string>() {"username"}),
                };

            public static IEnumerable<ApiResource> ApiResources =>
                new ApiResource[]
                {
                new ApiResource
                {
                    Name = "userservice",
                    DisplayName = "User Service",
                    Scopes = { "userservice.read", "userservice.write"}
                }
                };

            public static IEnumerable<ApiScope> ApiScopes =>
                new ApiScope[]
                {
                new ApiScope("userservice.read"),
                new ApiScope("userservice.write"),
                };

            public static IEnumerable<Client> Clients =>
                new Client[]
                {
                    //m2m client credentials flow client
                    new Client
                    {
                        ClientId = "userserviceapi",
                        ClientName = "User Service Api",

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                        AllowedScopes = { "userservice.read", "userservice.write" }
                    },

                    new Client
                    {
                        ClientId = "SPA",
                        ClientName = "My Angular App",
                        AllowedGrantTypes = GrantTypes.Implicit,

                        RedirectUris = { "http://localhost:4200/signin-callback" },
                        FrontChannelLogoutUri = "https://localhost:4200/signout-callback",

                        AllowedCorsOrigins = {"http://localhost:4200"},

                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = true,
                        AllowedScopes = { "openid", "profile" }

                    }
                };
        }
    }

}
