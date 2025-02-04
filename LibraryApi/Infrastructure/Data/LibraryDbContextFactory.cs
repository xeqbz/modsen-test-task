using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryApi.Infrastructure.Data
{
    public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=1234");
            return new LibraryDbContext(optionsBuilder.Options);
        }

    }
}
