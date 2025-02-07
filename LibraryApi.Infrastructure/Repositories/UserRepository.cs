using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace LibraryApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _dbContext;

        public UserRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
