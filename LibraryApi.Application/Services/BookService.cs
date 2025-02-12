using AutoMapper;
using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Domain.Common;
using LibraryApi.Application.Validators;
using Microsoft.AspNetCore.Hosting;
using LibraryApi.Application.Exceptions;
using LibraryApi.Application.Requests;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryApi.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly string _uploadPath;
        private readonly IWebHostEnvironment _enviroment;
        private readonly HttpClient _httpClient;

        public BookService(IBookRepository bookRepository, IMapper mapper, IWebHostEnvironment enviroment, IHttpClientFactory httpClientFactory)
        {

            _bookRepository = bookRepository;
            _mapper = mapper;
            _enviroment = enviroment;
            _httpClient = httpClientFactory.CreateClient();
            _uploadPath = enviroment.WebRootPath
                ?? throw new Exceptions.InvalidOperationException("WebRootPath is not correct");
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync()
                ?? throw new NotFoundException("No books found");
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");
            return _mapper.Map<BookDTO?>(book);
        }

        public async Task<BookDTO?> GetBookByISBNAsync(string isbn)
        {
            var book = await _bookRepository.GetByISBNAsync(isbn)
                ?? throw new NotFoundException("Book not found");
            return book == null ? null : _mapper.Map<BookDTO>(book);
        }

        public async Task<IEnumerable<BookDTO>> GetBooksByAuthorAsync(int authorId)
        {
            var books = await _bookRepository.GetBooksByAuthorAsync(authorId)
                ?? throw new NotFoundException("No books found");
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO> CreateBookAsync(CreateBookRequest dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _bookRepository.AddAsync(book);
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<bool> UpdateBookAsync(int id, CreateBookRequest dto)
        {
            var book = await _bookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");
            _mapper.Map(dto, book);
            await _bookRepository.UpdateAsync(book);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");
            await _bookRepository.DeleteAsync(book);
            return true;
        }

        public async Task<bool> UploadBookImageAsync(int bookId, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return false;

            var book = await _bookRepository.GetByIdAsync(bookId)
                ?? throw new NotFoundException("Book not found");

            var uploadsDirectory = Path.Combine(_uploadPath, "uploads");
            Directory.CreateDirectory(uploadsDirectory);

            var fileName = $"{bookId}_{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
            await File.WriteAllBytesAsync(filePath, imageBytes);

            await _bookRepository.UpdateImagePathAsync(bookId, $"/uploads/{fileName}");
            return true;
        }

        public async Task<bool> IssueBookAsync(int id, DateTimeOffset dueTo)
        {
            var book = await _bookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");

            if (book.IssuedAt != null)
                throw new Exceptions.InvalidOperationException("Book is already issued");
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

        public async Task<(byte[], string)> GetBookImageAsync(int bookId, IMemoryCache cache)
        {
            var book = await _bookRepository.GetByIdAsync(bookId)
                ?? throw new NotFoundException("Book not found");

            string cacheKey = $"book_image_{bookId}";
            return await cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                if (string.IsNullOrWhiteSpace(book.ImagePath))
                    throw new Exceptions.InvalidOperationException("Book does not have an image");

                var imagePath = Path.Combine(_uploadPath, book.ImagePath.TrimStart('/'));

                if (!File.Exists(imagePath))
                    throw new NotFoundException("Image file missing");

                return (await File.ReadAllBytesAsync(imagePath), book.ImagePath);
            });
        }

    }
}
