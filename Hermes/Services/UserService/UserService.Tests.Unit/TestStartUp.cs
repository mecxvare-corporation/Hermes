using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserService.Application.Mappers;

namespace UserService.Tests.Unit
{
    public class TestStartup
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            return services.BuildServiceProvider();
        }
    }
}
