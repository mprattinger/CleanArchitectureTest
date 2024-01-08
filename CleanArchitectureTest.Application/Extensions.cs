using CleanArchitectureTest.Data;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTest.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddData(configuration);

            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(Extensions)));
            services.AddValidatorsFromAssemblyContaining(typeof(Extensions));

            return services;
        }
    }
}
