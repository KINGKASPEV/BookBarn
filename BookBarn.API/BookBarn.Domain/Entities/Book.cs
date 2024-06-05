namespace BookBarn.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
    }
}
