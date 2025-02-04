using LibraryApi.Application.Interfaces;
using LibraryApi.Application.Mappings;
using LibraryApi.Application.Services;
using LibraryApi.Application.Validators;
using FluentValidation;
using LibraryApi.Infrastructure.Repositories;

namespace LibraryApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHttpClient();
            services.AddValidatorsFromAssembly(typeof(BookValidator).Assembly);
            return services;
        }
    }
}
