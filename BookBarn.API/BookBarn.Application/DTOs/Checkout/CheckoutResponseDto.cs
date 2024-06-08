namespace BookBarn.Application.DTOs.Checkout
{
    public class CheckoutResponseDto
    {
        public string CartId { get; set; }
        public string AppUserId { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
