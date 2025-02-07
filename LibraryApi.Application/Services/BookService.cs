using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Domain.Common;
using LibraryApi.Application.Validators;
using Microsoft.AspNetCore.Hosting;

namespace LibraryApi.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _enviroment;
        private readonly HttpClient _httpClient;

        public BookService(IBookRepository bookRepository, IMapper mapper, IWebHostEnvironment enviroment, IHttpClientFactory httpClientFactory)
        {

            _bookRepository = bookRepository;
            _mapper = mapper;
            _enviroment = enviroment;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return _mapper.Map<BookDTO?>(book);
        }

        public async Task<BookDTO?> GetBookByISBNAsync(string isbn)
        {
            var book = await _bookRepository.GetByISBNAsync(isbn);
            return book == null ? null : _mapper.Map<BookDTO>(book);
        }

        public async Task<IEnumerable<BookDTO>> GetBooksByAuthorAsync(int authorId)
        {
            var books = await _bookRepository.GetBooksByAuthorAsync(authorId);
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO> CreateBookAsync(CreateBookDTO dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _bookRepository.AddAsync(book);
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<bool> UpdateBookAsync(int id, CreateBookDTO dto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }
            _mapper.Map(dto, book);
            await _bookRepository.UpdateAsync(book);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }
            await _bookRepository.DeleteAsync(book);
            return true;
        }

        public async Task<bool> UploadBookImageAsync(int bookId, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return false;

            var webRootPath = _enviroment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
                throw new InvalidOperationException("WebRootPath is not configured properly.");

            var fileName = $"{bookId}_{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(webRootPath, "uploads", fileName);

            Directory.CreateDirectory(Path.Combine(webRootPath, "uploads"));
            var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
            await File.WriteAllBytesAsync(filePath, imageBytes);

            await _bookRepository.UpdateImagePathAsync(bookId, $"/uploads/{fileName}");
            return true;
        }

        public async Task<bool> IssueBookAsync(int id, DateTimeOffset dueTo)
        {
            return await _bookRepository.IssueBookAsync(id, dueTo);
        }

        public async Task<PagedResponse<BookDTO>> GetPagedBooksAsync(PaginationQuery paginationQuery)
        {
            var validator = new PaginationQueryValidator();
            var validationResult = await validator.ValidateAsync(paginationQuery);

            if (!validationResult.IsValid)
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var pagedBooks = await _bookRepository.GetPagedBooksAsync(paginationQuery);
            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(pagedBooks.Data);
            return new PagedResponse<BookDTO>(booksDTO, pagedBooks.PageNumber, pagedBooks.PageSize, pagedBooks.TotalItems);
        }
    }
}
