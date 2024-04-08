using HealthChecks.UI.Client;

using Hermes.Common;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using MongoDB.Driver;

using PostService.Api.Infrastructure.Middlewares;
using PostService.Api.Infrastructure.MongoDbSettings;
using PostService.Application.Mappers;
using PostService.Application.Posts.Commands;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Database;
using PostService.Infrastructure.Repositories;
using PostService.Infrastructure.Services;

using Serilog;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddSingleton<PostsStoreDatabase>();
builder.Services.AddSingleton<IPictureService, PictureService>();

builder.Services.Configure<PostsStoreDatabaseSettings>(
    builder.Configuration.GetSection("PostStoreDatabase"));

builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("PostStoreDatabase:ConnectionString")));

builder.Services.AddMediatR(
    config => config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(CreatePostCommand))!));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

builder.Configuration.AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetAssembly(typeof(PictureService))!, true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
