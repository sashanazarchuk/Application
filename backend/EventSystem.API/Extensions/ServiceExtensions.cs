namespace EventSystem.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerDocumentation();
            services.AddCorsPolicy();

            return services;
        }
    }
}