using LibraryApi.Domain;

namespace LibraryApi.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(LibraryDbContext context)
        {
            if (context.Books.Any()) return; 

            var books = new List<Book>
            {
                new Book { Id = 1, ISBN = "978-0451524935", Title = "1984", Genre = "Science Fiction", Description = "A dystopian novel by George Orwell", AuthorId = 1 },
                new Book { Id = 2, ISBN = "978-0060850524", Title = "Brave New World", Genre = "Science Fiction", Description = "A dystopian novel by Aldous Huxley", AuthorId = 2 }
            };

            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "George", LastName = "Orwell", Country = "United Kingdom", DateOfBirth = new DateTime(1903, 6, 25, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 2, FirstName = "Aldous", LastName = "Huxley", Country = "United Kingdom", DateOfBirth = new DateTime(1894, 7, 26, 0, 0, 0, DateTimeKind.Utc) }
            };

            context.Authors.AddRange(authors);
            context.Books.AddRange(books);
            context.SaveChanges(); 
        }
    }
}
