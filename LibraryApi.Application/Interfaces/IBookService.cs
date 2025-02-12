using LibraryApi.Application.DTOs;
using LibraryApi.Application.Requests;
using LibraryApi.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryApi.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO?> GetBookByIdAsync(int id);
        Task<BookDTO?> GetBookByISBNAsync(string isbn);
        Task<IEnumerable<BookDTO>> GetBooksByAuthorAsync(int authorId);
        Task<BookDTO> CreateBookAsync(CreateBookRequest dto);
        Task<bool> UpdateBookAsync(int id, CreateBookRequest dto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> UploadBookImageAsync(int id, string imageUrl);
        Task<bool> IssueBookAsync(int id, DateTimeOffset dueTo);
        Task<PagedResponse<BookDTO>> GetPagedBooksAsync(PaginationQuery paginationQuery);
        Task<(byte[], string)> GetBookImageAsync(int bookId, IMemoryCache cache);
    }
}
