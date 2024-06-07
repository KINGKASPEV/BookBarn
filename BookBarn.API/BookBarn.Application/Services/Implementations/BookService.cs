using AutoMapper;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain;
using BookBarn.Domain.Entities;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookBarn.Application.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<BookDto>> AddBookAsync(CreateBookDto createBookDto)
        {
            try
            {
                var book = _mapper.Map<Book>(createBookDto);
                await _bookRepository.AddAsync(book);
                await _bookRepository.SaveChangesAsync();
                var bookDto = _mapper.Map<BookDto>(book);
                return ApiResponse<BookDto>.Success(bookDto, "Book added successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a book.");
                return ApiResponse<BookDto>.Failed(false, "An error occurred while adding the book.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteBookAsync(string id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                {
                    return ApiResponse<bool>.Failed(false, "Book not found", 404, new List<string> { "Book not found" });
                }
                _bookRepository.Delete(book);
                await _bookRepository.SaveChangesAsync();
                return ApiResponse<bool>.Success(true, "Book deleted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting the book with id {id}.");
                return ApiResponse<bool>.Failed(false, "An error occurred while deleting the book.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<BookDto>>> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();
                var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
                return ApiResponse<IEnumerable<BookDto>>.Success(bookDtos, "Books retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all books.");
                return ApiResponse<IEnumerable<BookDto>>.Failed(false, "An error occurred while retrieving the books.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<BookDto>> GetBookByIdAsync(string id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                {
                    return ApiResponse<BookDto>.Failed(false, "Book not found", 404, new List<string> { "Book not found" });
                }
                var bookDto = _mapper.Map<BookDto>(book);
                return ApiResponse<BookDto>.Success(bookDto, "Book retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving the book with id {id}.");
                return ApiResponse<BookDto>.Failed(false, "An error occurred while retrieving the book.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<BookDto>> UpdateBookAsync(string bookId, UpdateBookDto updateBookDto)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    return ApiResponse<BookDto>.Failed(false, "Book not found", 404, new List<string> { "Book not found" });
                }
                _mapper.Map(updateBookDto, book);
                _bookRepository.Update(book);
                await _bookRepository.SaveChangesAsync();
                var bookDto = _mapper.Map<BookDto>(book);
                return ApiResponse<BookDto>.Success(bookDto, "Book updated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating the book with id {bookId}.");
                return ApiResponse<BookDto>.Failed(false, "An error occurred while updating the book.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<BookDto>>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                // Check if search term is provided
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return ApiResponse<IEnumerable<BookDto>>.Failed(false, "Search term cannot be empty", 400, new List<string> { "Search term cannot be empty" });
                }

                // Search books by title, author, year, or genre
                var books = await _bookRepository.FindAsync(book =>
                    book.Title.Contains(searchTerm) ||
                    book.Author.Contains(searchTerm) ||
                    book.PublicationYear.ToString().Contains(searchTerm) ||
                    book.Genre.Contains(searchTerm));

                if (books == null || !books.Any())
                {
                    return ApiResponse<IEnumerable<BookDto>>.Failed(false, "No books found matching the search criteria", 404, new List<string> { "No books found matching the search criteria" });
                }

                var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
                return ApiResponse<IEnumerable<BookDto>>.Success(bookDtos, "Books retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching books with term: {searchTerm}.");
                return ApiResponse<IEnumerable<BookDto>>.Failed(false, "An error occurred while searching books.", 500, new List<string> { ex.Message });
            }
        }

    }
}
