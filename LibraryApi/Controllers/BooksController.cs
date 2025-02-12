using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using LibraryApi.Domain.Common;
using LibraryApi.Application.Requests;

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
            return Ok(await _bookService.GetAllBooksAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBook(int id)
        {
            return Ok(await _bookService.GetBookByIdAsync(id));
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult> GetBookByISBN(string isbn)
        {
            return Ok(await _bookService.GetBookByISBNAsync(isbn));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateBook([FromBody] CreateBookRequest dto)
        {
            var book = await _bookService.CreateBookAsync(dto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] CreateBookRequest dto)
        {
            await _bookService.UpdateBookAsync(id, dto);
            return Ok("Book updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return Ok("Book deleted");
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult> GetBooksByAuthor(int authorId)
        {
            return Ok(await _bookService.GetBooksByAuthorAsync(authorId));
        }

        [HttpPost("{id}/uploadImage")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UploadBookImage(int id, [FromBody] string url)
        {
            await _bookService.UploadBookImageAsync(id, url);
            return Ok("File uploaded");
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetBookImage(int id, [FromServices] IMemoryCache cache)
        {
            var (imageBytes, fileName) = await _bookService.GetBookImageAsync(id, cache);
            return File(imageBytes, "image/jpeg", fileName);
        }


        [HttpPost("{id}/issue")]
        [Authorize]
        public async Task<ActionResult> IssueBook(int id, [FromBody] DateTimeOffset dueDate)
        {
            await _bookService.IssueBookAsync(id, dueDate);
            return Ok("Book issued");
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
