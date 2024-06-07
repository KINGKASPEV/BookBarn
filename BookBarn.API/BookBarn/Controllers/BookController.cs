using BookBarn.Application.DTOs.Book;
using BookBarn.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var response = await _bookService.AddBookAsync(createBookDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var response = await _bookService.DeleteBookAsync(id);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookService.GetAllBooksAsync();
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBookById(string id)
        {
            var response = await _bookService.GetBookByIdAsync(id);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("update/{bookId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(string bookId, [FromBody] UpdateBookDto updateBookDto)
        {
            var response = await _bookService.UpdateBookAsync(bookId, updateBookDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchBooks([FromQuery] string searchTerm)
        {
            var response = await _bookService.SearchBooksAsync(searchTerm);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
