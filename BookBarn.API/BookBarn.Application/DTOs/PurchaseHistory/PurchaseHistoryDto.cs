using BookBarn.Application.DTOs.Book;

namespace BookBarn.Application.DTOs.PurchaseHistory
{
    public class PurchaseHistoryDto
    {
        public string CartId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
