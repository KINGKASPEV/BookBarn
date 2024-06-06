using Microsoft.AspNetCore.Mvc;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Application.DTOs.Book;
using System.Threading.Tasks;

namespace BookBarn.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var response = await _bookService.AddBookAsync(createBookDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var response = await _bookService.DeleteBookAsync(id);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookService.GetAllBooksAsync();
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            var response = await _bookService.GetBookByIdAsync(id);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("update/{bookId}")]
        public async Task<IActionResult> UpdateBook(string bookId, [FromBody] UpdateBookDto updateBookDto)
        {
            var response = await _bookService.UpdateBookAsync(bookId, updateBookDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string searchTerm)
        {
            var response = await _bookService.SearchBooksAsync(searchTerm);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
