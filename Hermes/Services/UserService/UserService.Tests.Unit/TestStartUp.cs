using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserService.Application.Mappers;
using UserService.Application.Users.Commands;

namespace UserService.Tests.Unit
{
    public class TestStartup
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(CreateUserCommand))!));
            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            return services.BuildServiceProvider();
        }
    }
}
