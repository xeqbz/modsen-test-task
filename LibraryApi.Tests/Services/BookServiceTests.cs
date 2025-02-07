using AutoMapper;
using LibraryApi.Application.DTOs;
using LibraryApi.Application.Services;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Tests.Utils;
using LibraryApi.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace LibraryApi.Tests.Services
{
    public class BookServiceTests
    {
        private readonly BookService _bookService;
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly IMapper _mapper;

        public BookServiceTests()
        {
            _mockRepo = new Mock<IBookRepository>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockEnvironment.Setup(env => env.WebRootPath).Returns(Directory.GetCurrentDirectory());
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, BookDTO>();
                cfg.CreateMap<CreateBookDTO, Book>();
            });
            _mapper = config.CreateMapper();

            var httpClient = new HttpClient(new FakeHttpMessageHandler());
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _bookService = new BookService(_mockRepo.Object, _mapper, _mockEnvironment.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_Should_Return_BookList()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test1", ISBN = "1111111111" },
                new Book { Id = 2, Title = "Test2", ISBN = "2222222222" }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

            var result = await _bookService.GetAllBooksAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateBookAsync_Should_Create_Book()
        {
            var newBookDTO = new CreateBookDTO { Title = "Test", ISBN = "1234567890" };
            var newBook = new Book { Id = 1, Title = "Test", ISBN = "1234567890" };

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var result = await _bookService.CreateBookAsync(newBookDTO);

            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Book>()), Times.Once);
            Assert.Equal(newBookDTO.Title, result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_Should_Return_True_If_Successful()
        {
            var book = new Book { Id = 1, Title = "Old", ISBN = "1111111111" };
            var updatedDto = new CreateBookDTO { Title = "New", ISBN = "1111111111" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var result = await _bookService.UpdateBookAsync(1, updatedDto);

            Assert.True(result);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_Should_Return_False_If_Not_Found()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null);

            var result = await _bookService.UpdateBookAsync(1, new CreateBookDTO { Title = "New" });

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteBookAsync_Should_Return_True_If_Successful()
        {
            var book = new Book { Id = 1, Title = "Delete" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _mockRepo.Setup(repo => repo.DeleteAsync(book)).Returns(Task.CompletedTask);

            var result = await _bookService.DeleteBookAsync(1);

            Assert.True(result);
            _mockRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_Should_Return_False_If_Not_Found()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null);

            var result = await _bookService.DeleteBookAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task UploadBookImageByUrlAsync_Should_Return_True_When_Successful()
        {
            int bookId = 1;
            string imageUrl = "https://example.com/book.jpg";
            string expectedSavedPath = $"/uploads/{bookId}_";

            _mockRepo.Setup(repo => repo.UpdateImagePathAsync(It.IsAny<int>(), It.Is<string>(s => s.StartsWith(expectedSavedPath))))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _bookService.UploadBookImageAsync(bookId, imageUrl);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(repo => repo.UpdateImagePathAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task IssueBookAsync_Should_Return_True_When_Successful()
        {
            _mockRepo.Setup(repo => repo.IssueBookAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>()))
                     .ReturnsAsync(true);

            var result = await _bookService.IssueBookAsync(1, DateTimeOffset.UtcNow.AddDays(7));

            Assert.True(result);
        }

        [Fact]
        public async Task GetPagedBooksAsync_Should_Return_Correct_Data()
        {
            var books = new List<Book>
        {
            new Book { Id = 1, Title = "Test1", ISBN = "1111111111" },
            new Book { Id = 2, Title = "Test2", ISBN = "2222222222" }
        };

            var pagedResponse = new PagedResponse<Book>(books, 1, 10, 2);
            _mockRepo.Setup(repo => repo.GetPagedBooksAsync(It.IsAny<PaginationQuery>())).ReturnsAsync(pagedResponse);

            var result = await _bookService.GetPagedBooksAsync(new PaginationQuery { PageNumber = 1, PageSize = 10 });

            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_Should_Return_Book_If_Found()
        {
            var book = new Book { Id = 1, Title = "Test", ISBN = "1234567890" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);

            var result = await _bookService.GetBookByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(book.Title, result.Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_Should_Return_Null_If_Not_Found()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null);

            var result = await _bookService.GetBookByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetBookByISBN_Should_Return_Book()
        {
            var book = new Book { Id = 1, Title = "Test", ISBN = "1234567890" };
            _mockRepo.Setup(repo => repo.GetByISBNAsync("1234567890")).ReturnsAsync(book);

            var result = await _bookService.GetBookByISBNAsync("1234567890");

            Assert.NotNull(result);
            Assert.Equal(book.Title, result.Title);
        }

        [Fact]
        public async Task GetBooksByAuthorAsync_Should_Return_Books()
        {
            int authorId = 1;
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", ISBN = "1111111111", AuthorId = authorId },
                new Book { Id = 2, Title = "Book 2", ISBN = "2222222222", AuthorId = authorId }
            };

            _mockRepo.Setup(repo => repo.GetBooksByAuthorAsync(authorId)).ReturnsAsync(books);

            var result = await _bookService.GetBooksByAuthorAsync(authorId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, book => Assert.Equal(authorId, books.First(b => b.Id == book.Id).AuthorId));
        }

    }
}
