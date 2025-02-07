using LibraryApi.Domain.Entities;

namespace LibraryApi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
