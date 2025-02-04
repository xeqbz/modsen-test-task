using LibraryApi.Application.DTOs;
using LibraryApi.Domain;
using LibraryApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _dbContext;

        public BookRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _dbContext.Books.Include(b => b.Author).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _dbContext.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
        {
            return await _dbContext.Books.Include(b => b.Author).Where(b => b.AuthorId == authorId).ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateImagePathAsync(int id, string imagePath)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book != null)
            {
                book.ImagePath = imagePath;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IssueBookAsync(int id, DateTimeOffset dueTo)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null || book.IssuedAt != null)
            {
                return false;
            }

            book.IssuedAt = DateTimeOffset.UtcNow;
            book.DueTo = dueTo;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResponse<Book>> GetPagedBooksAsync(PaginationQuery paginationQuery)
        {
            var query = _dbContext.Books.Include(b => b.Author).AsQueryable();

            int totalItems = await query.CountAsync();
            var books = await query
                .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
                .Take(paginationQuery.PageSize)
                .ToListAsync();

            return new PagedResponse<Book>(books, paginationQuery.PageNumber, paginationQuery.PageSize, totalItems);
        }
    }
}
