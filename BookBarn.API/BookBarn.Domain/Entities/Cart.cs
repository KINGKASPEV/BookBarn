namespace BookBarn.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
    }
}
