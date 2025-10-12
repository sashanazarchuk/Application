using Microsoft.OpenApi.Models;

namespace EventSystem.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Management System", Version = "v1" });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Management System v1"));

            return app;
        }

        public static IApplicationBuilder UseSwaggerDev(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseSwaggerDocumentation();
            
            return app;
        }
    }
}