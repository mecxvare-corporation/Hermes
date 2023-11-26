using Hermes.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HermesDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("HermesDbConnection")));

builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IHermesRepository<>), typeof(HermesRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();