using BookBarn.Application.DTOs.Book;
using BookBarn.Domain;
using BookBarn.Domain.Entities;

namespace BookBarn.Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<ApiResponse<IEnumerable<BookDto>>> GetAllBooksAsync();
        Task<ApiResponse<BookDto>> GetBookByIdAsync(string id);
        Task<ApiResponse<BookDto>> AddBookAsync(CreateBookDto createBookDto);
        Task<ApiResponse<BookDto>> UpdateBookAsync(string bookId, UpdateBookDto updateBookDto);
        Task<ApiResponse<bool>> DeleteBookAsync(string id);
        Task<ApiResponse<IEnumerable<BookDto>>> SearchBooksAsync(string searchTerm);
    }
}
