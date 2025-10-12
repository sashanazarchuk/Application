namespace EventSystem.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerDocumentation();

            return services;
        }
    }
}
