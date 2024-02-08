using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Hermes.IdentityProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Infrastructure.Database
{
    public class IdentityDbInitializer
    {
        private readonly IdentityProviderDbContext _identityDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ILogger<IdentityProviderDbContext> _logger;

        public IdentityDbInitializer(IdentityProviderDbContext identityDbContext, ConfigurationDbContext configurationDbContext, ILogger<IdentityProviderDbContext> logger)
        {
            _identityDbContext = identityDbContext;
            _configurationDbContext = configurationDbContext;
            _logger = logger;
        }

        private readonly string userServiceSecret = "1c9857e4-620d-402d-9a3f-6f7973b3801a";
        private readonly string postServiceSecret = "519989b1-48b9-4b19-a2a7-9db877e24866";

        public async Task InitialiseAsync()
        {
            try
            {
                if (_identityDbContext.Database.IsNpgsql() && _configurationDbContext.Database.IsNpgsql())
                {
                    await _identityDbContext.Database.MigrateAsync();
                    await _configurationDbContext.Database.MigrateAsync();
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

        private async Task TrySeedAsync()
        {
            await SeedApiResources();

            await SeedIdentityResources();

            await SeedApiScopes();

            await SeedClients();

            await SeedUsers();
        }

        private async Task SeedUsers()
        {
            if (!_identityDbContext.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "admin@hermes.ge",
                        Password = "password",
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "dev@hermes.ge",
                        Password = "password",
                    }
                };

                await _identityDbContext.Users.AddRangeAsync(users);
                await _identityDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedClients()
        {
            if (!_configurationDbContext.Clients.Any())
            {
                var clients = new List<Client>
                {
                    // angular app
                    new Client
                    {
                        ClientId = "angular-client",
                        ClientName = "Angular Client",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        RedirectUris = {"http://localhost:4200/"}, // we will manage this in front-end
                        PostLogoutRedirectUris = {"http://localhost:4200/home"},
                        AllowedCorsOrigins = {"http://localhost:4200"},
                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                        },
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        //optionally
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = false,
                    }
                };

                foreach (var client in clients)
                {
                    await _configurationDbContext.Clients.AddAsync(client.ToEntity());
                }

                await _configurationDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedApiResources()
        {
            if (!_configurationDbContext.ApiResources.Any())
            {
                var apiResources = new List<ApiResource>
                {
                    new ApiResource("userservice", "User Service")
                    {
                        ApiSecrets = { new Secret(userServiceSecret.Sha256())},
                        Scopes = { "identityprovider" }
                    },

                    new ApiResource("postservice", "Post Service")
                    {
                        ApiSecrets = { new Secret(postServiceSecret.Sha256())},
                        Scopes = { "identityprovider" }
                    }
                };

                foreach (var resource in apiResources)
                {
                    await _configurationDbContext.ApiResources.AddAsync(resource.ToEntity());
                }

                await _configurationDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedIdentityResources()
        {
            if (!_configurationDbContext.IdentityResources.Any())
            {
                var identityResources = new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                };

                foreach (var res in identityResources)
                {
                    await _configurationDbContext.IdentityResources.AddAsync(res.ToEntity());
                }

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
                        Enabled = true,
                        Name = "userservice",
                        DisplayName = "User Service",
                        Description = "Allow access to user service",
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true
                    },

                    new ApiScope
                    {
                        Enabled = true,
                        Name = "identityprovider",
                        DisplayName = "Identity Provider",
                        Description = "Allow access to identity provider",
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true
                    },

                    new ApiScope
                    {
                        Enabled = true,
                        Name = "postservice",
                        DisplayName = "Post Service",
                        Description = "Allow access to post service",
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true
                    }
                };

                foreach (var apiScope in apiScopes)
                {
                    await _configurationDbContext.ApiScopes.AddRangeAsync(apiScope.ToEntity());
                }

                await _configurationDbContext.SaveChangesAsync();
            }
        }
    }
}
