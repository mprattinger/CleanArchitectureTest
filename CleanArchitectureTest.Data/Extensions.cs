using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTest.Data;

public static class Extensions
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(opt =>
        {
            opt.UseSqlite(configuration.GetConnectionString("ApplicationDb"))
            .EnableSensitiveDataLogging();
        });

        return services;
    }
}
