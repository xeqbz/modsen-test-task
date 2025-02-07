using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using LibraryApi.Domain.Common;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return book != null ? Ok(book) : NotFound();
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult> GetBookByISBN(string isbn)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn);
            return book != null ? Ok(book) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateBook([FromBody] CreateBookDTO dto)
        {
            var book = await _bookService.CreateBookAsync(dto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] CreateBookDTO dto)
        {
            var updated = await _bookService.UpdateBookAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteBookAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult> GetBooksByAuthor(int authorId)
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorId);
            return books.Any() ? Ok(books) : NotFound();
        }

        [HttpPost("{id}/uploadImage")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UploadBookImage(int id, [FromBody] string url)
        {
            var result = await _bookService.UploadBookImageAsync(id, url);
            return result ? Ok("File uploaded") : BadRequest("Error while uploading file");
        }

        [HttpGet("{id}/image")]
        public async Task<ActionResult> GetBookImage(int id, [FromServices] IMemoryCache cache)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null || string.IsNullOrEmpty(book.ImagePath))
                return NotFound("Image not found");

            string cacheKey = $"book_image_{id}";

            if (!cache.TryGetValue(cacheKey, out byte[] imageBytes))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.ImagePath.TrimStart('/'));

                if (!System.IO.File.Exists(imagePath))
                    return NotFound("Image file missing");

                imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                cache.Set(cacheKey, imageBytes, cacheOptions);
            }

            return File(imageBytes, "image/jpeg", $"{book.ImagePath}");
        }

        [HttpPost("{id}/issue")]
        [Authorize]
        public async Task<ActionResult> IssueBook(int id, [FromBody] DateTimeOffset dueDate)
        {
            var result = await _bookService.IssueBookAsync(id, dueDate);
            return result ? Ok("Book issued") : BadRequest("Error while issuing book");
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPagedBooks([FromQuery] PaginationQuery paginationQuery)
        {
            var result = await _bookService.GetPagedBooksAsync(paginationQuery);
            var totalPages = (int)Math.Ceiling((double)result.TotalItems / paginationQuery.PageSize);
            return Ok(new
            {
                Data = result.Data,
                PageNumber = result.PageNumber,
                result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = totalPages
            });
        }
    }
}
