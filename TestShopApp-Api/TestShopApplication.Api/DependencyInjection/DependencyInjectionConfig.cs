﻿using System;
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
            services.AddSingleton<AuthService>();
            return services;
        }
    }
}