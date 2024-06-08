namespace BookBarn.Domain.Entities
{
    public class Checkout : BaseEntity
    {
        public string CartId { get; set; }
        public Cart Cart { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }  // Adding this line
        public string PaymentMethod { get; set; } // "Web", "USSD", "Transfer"
        public string Status { get; set; } // Status of the checkout (e.g., "Success", "Failed")
        public DateTime? CompletedAt { get; set; }
        public string Message { get; set; }
    }
}
