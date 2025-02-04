using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string email, string password);
        Task<string> RefreshTokenAsync(string refreshToken);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
