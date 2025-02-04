using LibraryApi.API.Filters;

namespace LibraryApi.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
