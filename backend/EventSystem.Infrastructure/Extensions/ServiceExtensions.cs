using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Application.Services;
using EventSystem.Application.Settings;
using EventSystem.Infrastructure.Persistence.DataSeed;
using EventSystem.Infrastructure.Persistence.Repositories;
using EventSystem.Infrastructure.Profiles;
using EventSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            services.AddScoped<TagSeeder>();
            services.AddScoped<EventTagSeeder>();
            services.AddScoped<EventSeeder>();
            services.AddScoped<ParticipantSeeder>();

            //AI Settings
            services.Configure<AISettings>(configuration.GetSection("AISettings"));
            services.AddHttpClient<IAIService, AIService>();

            //JWT Settings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddJwtAuthentication(configuration.GetSection("JwtSettings").Get<JwtSettings>()!);
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            //AutoMapper Configuration
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AppUserProfile).Assembly));

            // Dependency Injection for Repositories and Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();
            services.AddSingleton<IPromptReaderService, PromptReaderService>();
            services.AddScoped<IUserSnapshotService, UserSnapshotService>();

            return services;
        }
    }
}