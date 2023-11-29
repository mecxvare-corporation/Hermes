using Hermes.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using UserService.Application.Users.Commands;
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

builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("UserServiceConnectionString"))); //scade aba aa is unda gaaketo imis implementacia daushvi

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddMediatR(Assembly.GetAssembly(typeof(CreateUserCommand)));
//builder.Services.AddAutoMapper();  // Fix this (Asembly)


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