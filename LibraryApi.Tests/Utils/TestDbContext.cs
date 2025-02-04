using LibraryApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Tests.Utils
{
    public static class TestDbContext
    {
        public static LibraryDbContext Create()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryDbTest")
                .Options;

            return new LibraryDbContext(options);
        }
    }
}
