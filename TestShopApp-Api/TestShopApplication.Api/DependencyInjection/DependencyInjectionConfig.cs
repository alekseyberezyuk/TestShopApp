using System;
using Microsoft.Extensions.DependencyInjection;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Api.Services;
using Microsoft.Extensions.Configuration;

namespace TestShopApplication.Api.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton<IAuthRepository>(sp => new AuthRepository(connectionString));
            services.AddSingleton<IItemsRepository>(sp => new ItemsRepository(connectionString));
            services.AddSingleton<IOrdersRepository>(sp => new OrdersRepository(connectionString));
            services.AddSingleton<IOrderRepository>(sp => new OrderRepository(connectionString));
            services.AddSingleton<AuthService>();
            services.AddSingleton<ItemsService>();
            services.AddSingleton<OrdersService>();
            return services;
        }
    }
}
