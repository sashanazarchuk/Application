using EventSystem.Infrastructure.Persistence.DataSeed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Database Configuration
            services.AddDatabase(configuration);

            //Identity Configuration
            services.ConfigureIdentity();

            //Register Seeders
            services.AddScoped<UserSeeder>();
            services.AddScoped<EventSeeder>();
            services.AddScoped<ParticipantSeeder>();

            return services;
        }
    }
}