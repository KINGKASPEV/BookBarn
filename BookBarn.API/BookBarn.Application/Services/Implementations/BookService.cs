using AutoMapper;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain;
using BookBarn.Domain.Entities;
using BookBarn.Infrastructure.Repositories.Interfaces;

namespace BookBarn.Application.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<BookDto>> AddBookAsync(CreateBookDto createBookDto)
        {
            var book = _mapper.Map<Book>(createBookDto);
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
            var bookDto = _mapper.Map<BookDto>(book);
            return ApiResponse<BookDto>.Success(bookDto, "Book added successfully", 201);
        }

        public async Task<ApiResponse<bool>> DeleteBookAsync(string id)
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

        public async Task<ApiResponse<IEnumerable<BookDto>>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
            return ApiResponse<IEnumerable<BookDto>>.Success(bookDtos, "Books retrieved successfully", 200);
        }

        public async Task<ApiResponse<BookDto>> GetBookByIdAsync(string id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return ApiResponse<BookDto>.Failed(false, "Book not found", 404, new List<string> { "Book not found" });
            }
            var bookDto = _mapper.Map<BookDto>(book);
            return ApiResponse<BookDto>.Success(bookDto, "Book retrieved successfully", 200);
        }

        public async Task<ApiResponse<BookDto>> UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            var book = await _bookRepository.GetByIdAsync(updateBookDto.Id);
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
    }
}
