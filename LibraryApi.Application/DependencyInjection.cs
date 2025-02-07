using LibraryApi.Application.Interfaces;
using LibraryApi.Application.Mappings;
using LibraryApi.Application.Services;
using LibraryApi.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace LibraryApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAutoMapper(typeof(BookProfile));
            services.AddAutoMapper(typeof(AuthorProfile));
            services.AddHttpClient();
            services.AddValidatorsFromAssembly(typeof(BookValidator).Assembly);
            return services;
        }
    }
}
