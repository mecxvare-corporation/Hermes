using Hermes.IdentityProvider.Domain;
using Hermes.IdentityProvider.Entities;
using Hermes.IdentityProvider.Infrastructure.Database;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hermes.IdentityProvider;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<IdentityProviderDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityServiceConnectionString")));

        builder.Services.AddScoped<IdentityDbInitializer>();
        builder.Services.AddScoped<IdentityProviderDbContextFactory>();

        builder.Services.AddAuthorization();

        var isBuilder = builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
            options.EmitStaticAudienceClaim = true;
        })
        .AddConfigurationStore(options =>
        options.ConfigureDbContext = b => b.UseNpgsql(
            builder.Configuration.GetConnectionString("IdentityServiceConnectionString"), opts => opts.MigrationsAssembly(typeof(User).Assembly.GetName().Name)))
        .AddOperationalStore(options =>
        options.ConfigureDbContext = b => b.UseNpgsql(
            builder.Configuration.GetConnectionString("IdentityServiceConnectionString"), opts => opts.MigrationsAssembly(typeof(User).Assembly.GetName().Name)))
        .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontendOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

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

        app.UseCors("AllowFrontendOrigin");
        app.UseIdentityServer();

        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        app.Run();

        return app;
    }
}
