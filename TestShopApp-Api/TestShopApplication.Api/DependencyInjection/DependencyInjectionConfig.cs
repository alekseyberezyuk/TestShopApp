using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Api.Services;
using TestShopApplication.Api.Validators;

namespace TestShopApplication.Api.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionString = connectionString.Replace("|DataDirectory|", configuration["appFolder"]).TrimEnd('\\');

            var assembly = typeof(IAuthRepository).Assembly;
            IEnumerable<(Type service, Type implementation)> typesToRegister = assembly
                .GetTypes()
                .Where(t => !t.IsInterface && t.Namespace?.EndsWith(".Dal.Repositories", StringComparison.OrdinalIgnoreCase) == true)
                .Select(t => (t?.GetInterfaces()?.FirstOrDefault(i => i.Name.Contains(t.Name)), t))
                .Where(r => r.Item1 != null);

            foreach (var (service, implementation) in typesToRegister)
            {
                var temp = Activator.CreateInstance(implementation, connectionString);
                services.AddSingleton(service, temp);
            }
            var servicesToRegister = typeof(AuthService).Assembly
                .GetTypes()
                .Where(t => t != null && t.Namespace.EndsWith("Api.Services", StringComparison.OrdinalIgnoreCase));

            foreach (var service in servicesToRegister)
            {
                services.AddSingleton(service);
            }
            services.AddSingleton<IItemsValidator, ItemsValidator>();
            return services;
        }
    }
}
