using LibraryApi.Application.DTOs;
using LibraryApi.Domain.Common;

namespace LibraryApi.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO?> GetBookByIdAsync(int id);
        Task<BookDTO?> GetBookByISBNAsync(string isbn);
        Task<IEnumerable<BookDTO>> GetBooksByAuthorAsync(int authorId);
        Task<BookDTO> CreateBookAsync(CreateBookDTO dto);
        Task<bool> UpdateBookAsync(int id, CreateBookDTO dto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> UploadBookImageAsync(int id, string imageUrl);
        Task<bool> IssueBookAsync(int id, DateTimeOffset dueTo);
        Task<PagedResponse<BookDTO>> GetPagedBooksAsync(PaginationQuery paginationQuery);
    }
}
