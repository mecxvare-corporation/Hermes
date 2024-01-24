using Hermes.IdentityProvider.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hermes.IdentityProvider;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // base-address of your identityserver
                options.Authority = "https://localhost:5001";

                // audience is optional, make sure you read the following paragraphs
                // to understand your options
                options.TokenValidationParameters.ValidateAudience = false;

                // it's recommended to check the type header to avoid "JWT confusion" attacks
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            });

        builder.Services.AddDbContext<IdentityProviderDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityServiceConnectionString")));

        builder.Services.AddIdentityServer(options =>
        {
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
            options.EmitStaticAudienceClaim = true;
        })
            .AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseNpgsql(builder.Configuration.GetConnectionString("IdentityServiceConnectionString"), opts => opts.MigrationsAssembly(typeof(Config).Assembly.GetName().Name)))
            .AddOperationalStore(options => options.ConfigureDbContext = b => b.UseNpgsql(builder.Configuration.GetConnectionString("IdentityServiceConnectionString"), opts => opts.MigrationsAssembly(typeof(Config).Assembly.GetName().Name)))
            /*.AddTestUsers(Config.TestUsers)*/;

        builder.Services.AddControllers();

        builder.Services.AddScoped<IdentityDbInitializer>();
        builder.Services.AddScoped<IdentityProviderDbContextFactory>();

        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            using (var scope = app.Services.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<IdentityDbInitializer>();

                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});

        app.Run();

        return app;
    }
}
