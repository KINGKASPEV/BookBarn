using AutoMapper;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.Services.Implementations;
using BookBarn.Domain.Entities;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace BookBarnTest.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IGenericRepository<Book>> _mockBookRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IGenericRepository<Book>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BookService>>();
            _bookService = new BookService(_mockBookRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddBookAsync_ShouldReturnSuccess_WhenBookIsAdded()
        {
            // Arrange
            var createBookDto = new CreateBookDto { Title = "Test Book", Author = "Author", Genre = "Genre", PublicationYear = 2021 };
            var book = new Book { Id = Guid.NewGuid().ToString(), Title = "Test Book", Author = "Author", Genre = "Genre", PublicationYear = 2021 };

            _mockMapper.Setup(m => m.Map<Book>(createBookDto)).Returns(book);
            _mockBookRepository.Setup(r => r.AddAsync(book)).Returns(Task.CompletedTask);
            _mockBookRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<BookDto>(book)).Returns(new BookDto { Id = book.Id, Title = book.Title, Author = book.Author, Genre = book.Genre, PublicationYear = book.PublicationYear });

            // Act
            var result = await _bookService.AddBookAsync(createBookDto);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Book added successfully", result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldReturnSuccess_WhenBookIsDeleted()
        {
            // Arrange
            var bookId = Guid.NewGuid().ToString();
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Author", Genre = "Genre", PublicationYear = 2021 };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mockBookRepository.Setup(r => r.Delete(book));
            _mockBookRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _bookService.DeleteBookAsync(bookId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Book deleted successfully", result.Message);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid().ToString();
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Author", Genre = "Genre", PublicationYear = 2021 };

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mockMapper.Setup(m => m.Map<BookDto>(book)).Returns(new BookDto { Id = book.Id, Title = book.Title, Author = book.Author, Genre = book.Genre, PublicationYear = book.PublicationYear });

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Book retrieved successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(bookId, result.Data.Id);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid().ToString();

            _mockBookRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Book not found", result.Message);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid().ToString(), Title = "Test Book 1", Author = "Author 1", Genre = "Genre 1", PublicationYear = 2021 },
                new Book { Id = Guid.NewGuid().ToString(), Title = "Test Book 2", Author = "Author 2", Genre = "Genre 2", PublicationYear = 2022 }
            };

            _mockBookRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(books);
            _mockMapper.Setup(m => m.Map<IEnumerable<BookDto>>(books)).Returns(books.Select(b => new BookDto { Id = b.Id, Title = b.Title, Author = b.Author, Genre = b.Genre, PublicationYear = b.PublicationYear }));

            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Books retrieved successfully", result.Message);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task SearchBooksAsync_ShouldReturnFailed_WhenSearchTermIsEmpty()
        {
            // Arrange
            var searchTerm = "";

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Search term cannot be empty", result.Message);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task SearchBooksAsync_ShouldReturnFailed_WhenNoBooksAreFound()
        {
            // Arrange
            var searchTerm = "Test";
            var books = new List<Book>();

            Expression<Func<Book, bool>> predicate = book =>
                book.Title.Contains(searchTerm) ||
                book.Author.Contains(searchTerm) ||
                book.PublicationYear.ToString().Contains(searchTerm) ||
                book.Genre.Contains(searchTerm);

            _mockBookRepository.Setup(r => r.FindAsync(predicate)).ReturnsAsync(books);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("No books found matching the search criteria", result.Message);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
