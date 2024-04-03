using HealthChecks.UI.Client;
using Hermes.Common;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

using PostService.Api.Infrastructure.Middlewares;
using PostService.Api.Infrastructure.MongoDbSettings;
using PostService.Domain.Interfaces;
using PostService.Infrastructure;
using PostService.Infrastructure.Database;
using PostService.Infrastructure.Repositories;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<PostsStoreDatabaseSettings>(
    builder.Configuration.GetSection("PostStoreDatabase"));

builder.Services.AddSingleton<IPostsStoreDatabaseSettings>(
    sp => sp.GetRequiredService<IOptions<PostsStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("PostStoreDatabase:ConnectionString")));

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
