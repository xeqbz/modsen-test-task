using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "George", LastName = "Orwell", Country = "United Kingdom", DateOfBirth = new DateTime(1903, 6, 25, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 2, FirstName = "Aldous", LastName = "Huxley", Country = "United Kingdom", DateOfBirth = new DateTime(1894, 7, 26, 0, 0, 0, DateTimeKind.Utc) }
            );
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, ISBN = "978-0451524935", Title = "1984", Genre = "Science Fiction", Description = "A dystopian novel by George Orwell", AuthorId = 1 },
                new Book { Id = 2, ISBN = "978-0060850524", Title = "Brave New World", Genre = "Science Fiction", Description = "A dystopian novel by Aldous Huxley", AuthorId = 2 }
            );
        }
    }
}
