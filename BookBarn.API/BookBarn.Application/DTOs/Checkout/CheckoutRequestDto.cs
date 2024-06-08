namespace BookBarn.Application.DTOs.Checkout
{
    public class CheckoutRequestDto
    {
        public string CartId { get; set; }
        public string PaymentMethod { get; set; } // "Web", "USSD", "Transfer"
    }
}
