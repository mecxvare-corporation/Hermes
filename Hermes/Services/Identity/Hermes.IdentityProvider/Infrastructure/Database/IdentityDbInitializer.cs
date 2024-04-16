using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Hermes.IdentityProvider.Entities;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Infrastructure.Database
{
    public class IdentityDbInitializer
    {
        private readonly ILogger<IdentityProviderDbContext> _logger;
        private readonly IdentityProviderDbContext _context;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;

        public IdentityDbInitializer(ILogger<IdentityProviderDbContext> logger, IdentityProviderDbContext context, ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext)
        {
            _logger = logger;
            _context = context;
            _configurationDbContext = configurationDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsNpgsql())
                {
                    await _context.Database.MigrateAsync();
                }
                if (_configurationDbContext.Database.IsNpgsql())
                {
                    await _configurationDbContext.Database.MigrateAsync();
                }
                if (_persistedGrantDbContext.Database.IsNpgsql())
                {
                    await _persistedGrantDbContext.Database.MigrateAsync();
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
            await SeedUsers();
            await SeedClients();
            await SeedIdentityResources();
            await SeedApiResources();
            await SeedApiScopes();
        }

        private async Task SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { UserName = "admin", FirstName = "jora", LastName = "joradze", DateOfBirth = new DateTime(1990, 1, 15, 0, 0, 0, DateTimeKind.Utc), Email = "admin@hermes.ge", Password = "password".ToSha256() }.AddUserClaims(new Entities.UserClaim ("role", "admin")),
                    new User { UserName = "dato", FirstName = "ramazi", LastName = "bluyunadze", DateOfBirth = new DateTime(1985, 6, 20, 0, 0, 0, DateTimeKind.Utc), Email = "dato@hermes.ge", Password = "password".ToSha256() }.AddUserClaims(new Entities.UserClaim ("role", "dev")),
                    new User { UserName = "gvantsa", FirstName = "taxmagido", LastName = "zoroshvili", DateOfBirth = new DateTime(1978, 9, 5, 0, 0, 0, DateTimeKind.Utc), Email = "dato@hermes.ge", Password = "password".ToSha256() }.AddUserClaims(new Entities.UserClaim ("role", "dev")),
                    new User { UserName = "bacho",FirstName = "dundula", LastName = "trakadze", DateOfBirth = new DateTime(2000, 3, 10, 0, 0, 0, DateTimeKind.Utc), Email = "dato@hermes.ge", Password = "password".ToSha256() }.AddUserClaims(new Entities.UserClaim ("role", "dev")),
                    new User { UserName = "giorgi", FirstName = "guja", LastName = "notariusi", DateOfBirth = new DateTime(1995, 11, 30, 0, 0, 0, DateTimeKind.Utc), Email = "dato@hermes.ge", Password = "password".ToSha256() }.AddUserClaims(new Entities.UserClaim ("role", "dev")),
                };

                await _context.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedClients()
        {
            if (!_configurationDbContext.Clients.Any())
            {
                var clients = new List<Client>
                {
                    new Client
                    {

                        ClientId = "SPA",
                        ClientName = "My Angular App",
                        AllowedGrantTypes = new List<ClientGrantType>{ new ClientGrantType { GrantType = "implicit" } },

                        RedirectUris = new List<ClientRedirectUri> { new ClientRedirectUri { RedirectUri = "http://localhost:4200/signin-callback" } },
                        PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                        {
                            new ClientPostLogoutRedirectUri{ PostLogoutRedirectUri = "http://localhost:4200/signout-callback" }
                        },
                        AllowedCorsOrigins = new List<ClientCorsOrigin>{ new ClientCorsOrigin { Origin = "http://localhost:4200" } },
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = true,
                        AllowedScopes = new List<ClientScope>{
                            new ClientScope { Scope = "openid"},
                            new ClientScope { Scope = "profile"},
                            new ClientScope { Scope = "userserviceapi" }
                        },
                        AllowOfflineAccess = true,
                    }
                };

                await _configurationDbContext.AddRangeAsync(clients);
                await _configurationDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedIdentityResources()
        {
            if (!_configurationDbContext.IdentityResources.Any())
            {
                var identityResources = new List<IdentityResource>
                {
                    new IdentityResource
                    {
                        Name = IdentityServerConstants.StandardScopes.OpenId,
                        DisplayName = "Your user identifier",
                        Required = true,
                        UserClaims = new List<IdentityResourceClaim> { new IdentityResourceClaim { Type = "sub"} }
                    },
                    new IdentityResource
                    {
                        Name = IdentityServerConstants.StandardScopes.Profile,
                        DisplayName = "User profile",
                        Description = "Your user profile information (first name, last name, etc.)",
                        Emphasize = true,
                        UserClaims = new List<IdentityResourceClaim> { new IdentityResourceClaim { Type = "profile" } }
                    },
                    new IdentityResource
                    {
                        Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                        UserClaims = new List<IdentityResourceClaim> { new IdentityResourceClaim { Type = "offline_access" } }
                    }
                };

                await _configurationDbContext.AddRangeAsync(identityResources);
                await _configurationDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedApiResources()
        {
            if (!_configurationDbContext.ApiResources.Any())
            {
                var apiResources = new List<ApiResource>
                {
                    new ApiResource
                    {
                        Name = "userservice",
                        DisplayName = "User Service",
                        Scopes = new List<ApiResourceScope>{ new ApiResourceScope { Scope = "userservice.read" } , new ApiResourceScope { Scope ="userservice.write"} },
                    }
                };

                await _configurationDbContext.AddRangeAsync(apiResources);
                await _configurationDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedApiScopes()
        {
            if (!_configurationDbContext.ApiScopes.Any())
            {
                var apiScopes = new List<ApiScope>
                {
                    new ApiScope
                    {
                        Name = "userservice.read",
                        Description = "userservice.read",
                        Enabled = true,
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true
                    },
                    new ApiScope
                    {
                        Name = "userservice.write",
                        Description = "userservice.write",
                        Enabled = true,
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true
                    }
                };

                await _configurationDbContext.AddRangeAsync(apiScopes);
                await _configurationDbContext.SaveChangesAsync();
            }
        }
    }
}

