namespace BookBarn.Application.DTOs.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
