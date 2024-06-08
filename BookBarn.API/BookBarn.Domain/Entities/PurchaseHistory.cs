namespace BookBarn.Domain.Entities
{
    public class PurchaseHistory : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
