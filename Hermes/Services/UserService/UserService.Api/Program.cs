using HealthChecks.UI.Client;
using Hermes.Common;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using UserService.Api.Infrastructure.Middlewares;
using UserService.Application.Mappers;
using UserService.Application.Users.Commands;
using UserService.Domain.Interfaces;
using UserService.Infrastructure;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("UserServiceConnectionString")));


builder.Services.AddScoped<UserServiceDbContextInitialiser>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInterestRepository, InterestRepository>();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(CreateUserCommand))!));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("UserServiceConnectionString")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<UserServiceDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();