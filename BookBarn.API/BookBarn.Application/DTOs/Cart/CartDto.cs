using BookBarn.Application.DTOs.Book;

namespace BookBarn.Application.DTOs.Cart
{
    public class CartDto
    {
        public string Id { get; set; }
        public string AppUserId { get; set; }
        public ICollection<BookDto> Books { get; set; }
    }
}
