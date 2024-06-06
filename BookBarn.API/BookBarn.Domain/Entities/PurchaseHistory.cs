namespace BookBarn.Domain.Entities
{
    public class PurchaseHistory : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<Book> Books { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}
