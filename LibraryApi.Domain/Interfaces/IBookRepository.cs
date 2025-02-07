using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Common;

namespace LibraryApi.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task UpdateImagePathAsync(int id, string imagePath);
        Task<bool> IssueBookAsync(int id, DateTimeOffset dueTo);
        Task<PagedResponse<Book>> GetPagedBooksAsync(PaginationQuery paginationQuery);
    }
}
