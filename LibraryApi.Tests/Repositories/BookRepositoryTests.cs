using LibraryApi.Domain.Entities;
using LibraryApi.Infrastructure.Data;
using LibraryApi.Infrastructure.Repositories;
using LibraryApi.Tests.Utils;

namespace LibraryApi.Tests.Repositories
{
    public class BookRepositoryTests
    {
        private readonly LibraryDbContext _context;
        private readonly BookRepository _bookRepository;

        public BookRepositoryTests()
        {
            _context = TestDbContext.Create();
            _bookRepository = new BookRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Book_To_Db()
        {
            var book = new Book { Id = 1, Title = "Test", ISBN = "123456789" };

            await _bookRepository.AddAsync(book);
            var savedBook = await _context.Books.FindAsync(1);

            Assert.NotNull(savedBook);
            Assert.Equal(book.Title, savedBook.Title);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Book()
        {
            var book = new Book { Id = 2, Title = "Test2", ISBN = "0987654321" };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _bookRepository.GetByIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal(book.Title, result.Title);
        }
    }
}
