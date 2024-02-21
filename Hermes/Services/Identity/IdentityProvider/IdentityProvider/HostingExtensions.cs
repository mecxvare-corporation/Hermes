using Duende.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityProvider
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();

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
                    builder.Configuration.GetConnectionString("IdentityServiceConnectionString"), opts => opts.MigrationsAssembly(typeof(Config).Assembly.GetName().Name)))
                .AddOperationalStore(options =>
                options.ConfigureDbContext = b => b.UseNpgsql(
                    builder.Configuration.GetConnectionString("IdentityServiceConnectionString"), opts => opts.MigrationsAssembly(typeof(Config).Assembly.GetName().Name)))
                .AddTestUsers(TestUsers.Users);


            // if you want to use server-side sessions: https://blog.duendesoftware.com/posts/20220406_session_management/
            // then enable it
            //isBuilder.AddServerSideSessions();
            //
            // and put some authorization on the admin/management pages
            //builder.Services.AddAuthorization(options =>
            //       options.AddPolicy("admin",
            //           policy => policy.RequireClaim("sub", "1"))
            //   );
            //builder.Services.Configure<RazorPagesOptions>(options =>
            //    options.Conventions.AuthorizeFolder("/ServerSideSessions", "admin"));


            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.MapRazorPages()
                .RequireAuthorization();

            return app;
        }
    }
}